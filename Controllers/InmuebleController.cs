using devs.Models;
using Microsoft.AspNetCore.Mvc;

namespace devs.Controllers
{
    public class InmuebleController : Controller
    {

        private readonly RepositorioInmueble _repositorio;
        private readonly RepositorioPropietario _repositorioP;

        public InmuebleController(RepositorioInmueble repositorio, RepositorioPropietario repositorioP)
        {
            _repositorio = repositorio;
            _repositorioP = repositorioP;
        }


        [Route("[controller]/Buscar/{q}")]
        public IActionResult Buscar(string q)
        {
            try
            {
                var res = _repositorio.buscarPorNombre(q);
                return Json(new { results = res });
            }
            catch(Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }
        // GET: InmuebleController
        public ActionResult Index()
        {
            var lista = _repositorio.ObtenerTodos();

            return View(lista);
        }


        public ActionResult Crear()
        {
            ViewBag.Propietarios = _repositorioP.verTodos();
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Inmueble inm)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    _repositorio.Alta(inm);
                    TempData["Message"] = "Inmueble creado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Propietarios = _repositorioP.verTodos();
                return View(inm);
            }
            catch (Exception ex)
            {

                TempData["Message"] = "error al crear el inmueble";
                ViewBag.Propietarios = _repositorioP.verTodos();
                return View(inm);
            }


        }


        [HttpGet]
        public ActionResult Modificar(int id)
        {
            try
            {

                //ViewBag.Propietarios = _repositorioP.verTodos();
                var inmueble = _repositorio.BuscarPorId(id);
                if (inmueble == null)
                {

                    TempData["Message"] = "No se encontro el inmueble";
                    return RedirectToAction(nameof(Index));
                }

                return View(inmueble);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "No se encontro el inmueble";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modificar(Inmueble inm)
        {
            try
            {
                //ViewBag.Propietarios = _repositorioP.verTodos();
                if (ModelState.IsValid)
                {

                    _repositorio.ModificarInmueble(inm);
                    TempData["Message"] = "inmueble modificado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                return View(inm);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "No se pudo modificar el inmueble";
                return View(inm);

            }

        }

        [HttpPost]
        public ActionResult Borrar(int id)
        {
            try
            {
                _repositorio.BajaLogica(id);
                TempData["Message"] = " Inmueble borrado con exito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "No se pudo borrar el inmueble";


            }
            return RedirectToAction(nameof(Index));



        }

        public ActionResult Detalles(int id, [FromServices] RepositorioContrato repoContrato)
        {
            var inmueble = _repositorio.BuscarPorId(id);
            if (inmueble == null)
            {
                TempData["Message"] = "No se encontró el inmueble.";
                return RedirectToAction(nameof(Index));

            }
            inmueble.ContratosInmueble = repoContrato.ObtenerContratosPorInmueble(id);
            return View(inmueble);

        }

    }
}
