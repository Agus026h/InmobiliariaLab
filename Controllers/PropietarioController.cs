using Microsoft.AspNetCore.Mvc;
using devs.Models;


namespace devs.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly RepositorioPropietario _repositorio;

        public PropietarioController(RepositorioPropietario repositorio)
        {
            _repositorio = repositorio;
        }

        // GET: Propietario
        public IActionResult Index(int pagina = 1, string dni = null, string estado = null)
        {
            var tamanio = 5;
                pagina = (Math.Max(pagina, 1));

                bool? estadoFiltro = null;
                if (!string.IsNullOrEmpty(estado) && bool.TryParse(estado, out bool parsedEstado))
                {
                 estadoFiltro = parsedEstado;
                }
                var (lista, total) = _repositorio.verTodosPaginado(pagina, tamanio, dni, estadoFiltro);
                ViewBag.Pagina = pagina;
                ViewBag.TotalPaginas = total % tamanio == 0 ? total / tamanio : (total / tamanio) + 1;

                ViewBag.DniFiltro = dni;
                ViewBag.EstadoFiltro = estado;

           
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_TablaPropietariosPartial", lista);
            }

            return View(lista);
        }



        public ActionResult Crear()
        {


            return View();
        }



        // POST: Propietario/Crear
        [HttpPost] // para el submit

        public IActionResult Procesar(Propietario p, string accion)
        {
            try
            {
                switch (accion)
                {
                    case "Buscar":
                        ModelState.Clear();
                        var persona = _repositorio.buscarId(p.IdPropietario);
                        if (persona != null)
                            ViewBag.Mensaje = "Persona encontrada!" + persona.Nombre;

                        else
                            ViewBag.Mensaje = "No se encontro la persona con ese id";
                        return View("Crear", persona);

                    case "Crear":

                        _repositorio.Alta(p);
                        ViewBag.Mensaje = "Propietario creado con exito";
                        break;

                    case "Modificar":

                        _repositorio.ModificarPropietario(p);
                        ViewBag.Mensaje = "Propietario modificado con exito";
                        break;

                }
            }
            catch (Exception ex)
            {
                ViewBag.message = "Error al realizar la accion";
            }
            return View("Crear", p);
        }


        /*
        [ValidateAntiForgeryToken]

        public ActionResult Crear(Propietario p)
        {
            if (ModelState.IsValid)// se fija si el modelo es valido
                _repositorio.Alta(p);

            return View();
        }

       */

        [HttpGet]
        public ActionResult Editar(int id)
        {
            try
            {
                var propietario = _repositorio.buscarId(id);
                if (propietario == null)
                {
                    TempData["Message"] = "No se encontro el propietario con ese ID";
                    return RedirectToAction(nameof(Index));
                }


                return View("Crear", propietario);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error al cargar el propietario: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        public ActionResult Borrar(int id)
        {
            try
            {
                _repositorio.Baja(id);
                TempData["Message"] = " Propietario borrado con exito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData["Message"] = " Error al borrar Propietario";
                return RedirectToAction(nameof(Index));
            }


        }
        [Route("[controller]/Buscar/{q}", Name = "Buscar")]
        public IActionResult Buscar(string q)
        {
            try
            {
                var res = _repositorio.buscarPorNombre(q);
                return Json(new { Datos = res });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }

        public ActionResult Detalles(int id, [FromServices] RepositorioInmueble repoInmueble)
        {
            try
            {
                var propietario = _repositorio.buscarId(id);


                if (propietario == null)
                {
                    TempData["Message"] = "No se encontro al propietario.";
                    return RedirectToAction(nameof(Index));
                }
                propietario.InmueblesPropietario = repoInmueble.ObtenerPorPropietario(id);

                return View(propietario);
            }
            catch (Exception ex)
            {

                TempData["Message"] = " Error al cargar detalles";
                return RedirectToAction(nameof(Index));
            }
        }
    }




}