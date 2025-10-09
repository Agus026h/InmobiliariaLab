using devs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;

namespace devs.Controllers
{
    //[Authorize(Policy = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> logger;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly RepositorioUsuario repositorio;

        public UsuarioController(IConfiguration configuration, IWebHostEnvironment environment, RepositorioUsuario repositorio, ILogger<UsuarioController> logger)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
            this.logger = logger;
        }
        //GET: Usuario
        public ActionResult Index(int pagina = 1)
        {
            var usuarios = repositorio.ObtenerLista(pagina);
            return View(usuarios);
        }

        [AllowAnonymous]
        //Get: Usuario/Login
        public ActionResult Login(string returnUrl)//returnUrl es para redirigirlo a la pagina que buscaba 
        {
            TempData["returnUrl"] = returnUrl;
            return View();

        }
        //GET: Usuario/Crear
        [AllowAnonymous]
        public ActionResult Crear()
        {

            return View();
        }
        //Get: Usuarios/Edit                 
        public ActionResult Perfil()
        {
            ViewData["Title"] = "Mi perfil";
            var u = repositorio.ObtenerPorEmail(User.Identity.Name);
            return View("Edit", u);

        }
        //GET: Usuario/Edit
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            ViewData["Title"] = "Editar Usuario";
            var u = repositorio.ObtenerPorId(id);
            return View(u);
        }
        //POST: Usuario/Edit
        [HttpPost]
		[ValidateAntiForgeryToken]
		
		public async Task<ActionResult> Edit(int idUsuario, Usuario u)
		{
			var vista = nameof(Edit);
            if (!ModelState.IsValid)
            {
               
                u.IdUsuario = idUsuario; 
                return View(vista, u); 
            }
			try
            {
                var emailLogeado = User?.Identity?.Name;
                var usuarioLogeadoBD = String.IsNullOrEmpty(emailLogeado) ? null : repositorio.ObtenerPorEmail(emailLogeado);//traigo usuario de la base de datos
                
                if (!User.IsInRole("Administrador"))
                {
                    vista = nameof(Perfil);

                    if (usuarioLogeadoBD?.IdUsuario != idUsuario)
                        return RedirectToAction(nameof(Index), "Home");
                }
                var usuarioOriginal = repositorio.ObtenerPorId(idUsuario);
                if (User.IsInRole("Administrador"))
                {
                    
                    if (!string.IsNullOrEmpty(u.Clave))
                    {
                        //paso la clave que proporciono el administrador
                        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: u.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"] ?? ""),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));
                        u.Clave = hashed;
                    }
                    else
                    {
                        u.Clave = usuarioOriginal.Clave;
                    }
                }
                if (u.AvatarFile != null)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");


                    if (!string.IsNullOrEmpty(usuarioOriginal.Avatar))
                    {
                        var oldPath = Path.Combine(wwwPath, usuarioOriginal.Avatar.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }


                    string fileName = "avatar_" + u.IdUsuario + Path.GetExtension(u.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Avatar = Path.Combine("/Uploads", fileName);

                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        await u.AvatarFile.CopyToAsync(stream);
                    }
                }
                else
                {
                    u.Avatar = usuarioOriginal.Avatar;
                }

                if (!User.IsInRole("Administrador"))
                {
                    u.Clave = usuarioOriginal.Clave;
                    u.Rol = usuarioOriginal.Rol;
                }
                repositorio.Modificacion(u);
                //si el usuario se modifica a si mismo se actualizan las claims
                if (usuarioLogeadoBD?.IdUsuario == idUsuario)
                {
                    var claims = new List<Claim>
                {
                    // paso los datos actualizados de u
                    new Claim(ClaimTypes.Name, u.Email),
                    new Claim("FullName", u.Nombre + " " + u.Apellido),
                    new Claim(ClaimTypes.Role, u.Rol.ToString()),
                    new Claim("IdUsuario", u.IdUsuario.ToString()), 
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal
                    );
                }
                    TempData["Message"] = "Los datos del usuario se han actualizado correctamente.";
                   
                    return RedirectToAction(vista);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al editar el usuario");
                TempData["Message"] = "Error al editar usuario" + ex;
                

                return View(vista, u);
            }
		}


        //Post: Usuario/crear
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Crear(Usuario u)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: u.Clave,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"] ?? ""),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                u.Clave = hashed;

                int res = repositorio.Alta(u);
                if (u.AvatarFile != null && u.IdUsuario > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string fileName = "avatar_" + u.IdUsuario + Path.GetExtension(u.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Avatar = Path.Combine("/Uploads", fileName);

                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);
                    }
                    repositorio.Modificacion(u);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al crear el usuario");
                ViewBag.Error = ex.Message;

                return View();
            }
        }

        //Post: Usuario/Login/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as String) ? "/Home" : (TempData["returnUrl"] ?? "").ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                       password: login.Clave,
                       salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"] ?? ""),
                       prf: KeyDerivationPrf.HMACSHA1,
                       iterationCount: 1000,
                       numBytesRequested: 256 / 8
                    ));

                    var e = repositorio.ObtenerPorEmail(login.Usuario);
                    if (e == null)
                    {
                        ModelState.AddModelError("", "El email es incorrecto");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }
                    else if (e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "La clave es incorrecta");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }
                    //no informa cual dato es incorrecto, es una forma mas segura
                    // if (e == null || e.Clave != hashed)
                    // {
                    // 	ModelState.AddModelError("", "El email o la clave no son correctos");
                    // 	TempData["returnUrl"] = returnUrl;
                    // 	return View();
                    // }
                    var claims = new List<Claim>
                    {
                        new Claim("IdUsuario",e.IdUsuario.ToString()),//guardo el id
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.Rol.ToString()),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);


                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }







        //GET: salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }
        //Post: Usuario/Delete
        [HttpPost]
        

        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
                var ruta = Path.Combine(environment.WebRootPath, "Uploads", $"avatar_{id}" + Path.GetExtension(usuario.Avatar));
                if (System.IO.File.Exists(ruta))
                    System.IO.File.Delete(ruta);
                repositorio.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al eliminar el usuario");
                return RedirectToAction(nameof(Index));
            }
        }
       







    }
}