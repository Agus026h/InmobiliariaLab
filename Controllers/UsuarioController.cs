using devs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace devs.Controllers
{
    [Authorize]
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
        [Authorize(Policy = "Administrador")]
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
        
        [Authorize(Policy = "Administrador")]
        public ActionResult Crear()
        {

            return View();
        }
        //Get: Usuarios/Edit   
                     
        public ActionResult Perfil()
        {
            ViewBag.CambioClave = new devs.Models.CambioClaveView();
            ViewData["Title"] = "Mi perfil";
            var u = repositorio.ObtenerPorEmail(User.Identity.Name);
            return View("Edit", u);

        }
        //GET: Usuario/Edit
        
        public ActionResult Edit(int id)
        {
            ViewData["Title"] = "Editar Usuario";
            var u = repositorio.ObtenerPorId(id);
            return View(u);
        }
        //POST: Usuario/Edit

         
        [HttpPost]
        [ActionName("Edit")]
		[ValidateAntiForgeryToken]
		
		public async Task<ActionResult> GuardarPerfil(int idUsuario, Usuario u)
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
                   
                    return View(vista, u);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al editar el usuario");
                TempData["Message"] = "Error al editar usuario" + ex;
                

                return View(vista, u);
            }
		}
       
        [Authorize(Policy = "Administrador")]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarClave(CambioClaveView modelo)
        {
            
            if (!ModelState.IsValid)
            {
                ViewBag.CambioClave = modelo;
                TempData["Message"] = "ERROR: Por favor, revisa los datos ingresados";
                return View(nameof(Edit), repositorio.ObtenerPorEmail(User.Identity.Name));
            }

            try
            {
                var emailLogeado = User?.Identity?.Name;
                var usuarioDB = repositorio.ObtenerPorEmail(emailLogeado);

                if (usuarioDB == null)
                {
                    TempData["Message"] = "Error: Usuario no encontrado";
                    return RedirectToAction(nameof(Index), "Home");
                }

                string hashClaveViejaIngresada = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: modelo.ClaveVieja,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"] ?? ""),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                
                if (hashClaveViejaIngresada != usuarioDB.Clave)
                {
                    ModelState.AddModelError(string.Empty, "La Contraseña Actual ingresada es incorrecta");
                    TempData["Message"] = "ERROR: La Contraseña Actual ingresada es incorrecta";
                    ViewBag.CambioClave = modelo;
                   
                    return View(nameof(Edit), repositorio.ObtenerPorEmail(User.Identity.Name));
                }

            
                string hashClaveNueva = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: modelo.ClaveNueva,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"] ?? ""),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                
                repositorio.ActualizarClave(usuarioDB.IdUsuario, hashClaveNueva);

              
                TempData["Message"] = "Contraseña cambiada exitosamente.";
                ViewBag.CambioClave = new CambioClaveView();

                
                return RedirectToAction(nameof(Perfil));
            }
            catch (Exception ex)
            {
                
                logger.LogError(ex, "Error al cambiar la clave del usuario {UserId}", User?.Identity?.Name);
                TempData["Message"] = "Error interno al procesar el cambio de clave.";

                
                return RedirectToAction(nameof(Perfil));
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
        
        [Authorize(Policy = "Administrador")]
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