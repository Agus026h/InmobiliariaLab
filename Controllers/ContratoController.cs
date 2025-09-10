using devs.Models;
using Microsoft.AspNetCore.Mvc;

namespace devs.Controllers
{
    public class ContratoController : Controller
    {

        private readonly RepositorioContrato _repositorio;
        private readonly RepositorioInmueble _repositorioInmueble;
        private readonly RepositorioInquilino _repositorioInquilino;

        public ContratoController(
            RepositorioContrato repositorio,
            RepositorioInmueble repositorioInm,
            RepositorioInquilino repositorioInq)
        {
            _repositorio = repositorio;
            _repositorioInmueble = repositorioInm;
            _repositorioInquilino = repositorioInq;

        }

        // GET: ContratoController
        public ActionResult Index()
        {
            var lista = _repositorio.VerTodos();
            return View(lista);
        }

        public ActionResult Crear()
        {

            ViewBag.Inmuebles = _repositorioInmueble.ObtenerTodos();
            //usa ver todos pero se podria filtar con ver activos
            ViewBag.Inquilinos = _repositorioInquilino.verTodos();
            return View();

        }
        [HttpPost]
        public ActionResult Crear(Contrato c)
        {
            ViewBag.Inmuebles = _repositorioInmueble.ObtenerTodos();
            ViewBag.Inquilinos = _repositorioInquilino.verTodos();
            if (ModelState.IsValid)
            {
                _repositorio.Alta(c);
                TempData["Message"] = "contrato creado correctamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["Message"] = "error al crear el contrato";
            return View(c);

        }


        [HttpGet]
        public ActionResult Modificar(int id)
        {
            ViewBag.Inmuebles = _repositorioInmueble.ObtenerTodos();
            ViewBag.Inquilinos = _repositorioInquilino.verTodos();
            var contra = _repositorio.BuscarId(id);
            return View(contra);


        }


        [HttpPost]
        public ActionResult Modificar(Contrato c)
        {
            try
            {
                ViewBag.Inmuebles = _repositorioInmueble.ObtenerTodos();
                ViewBag.Inquilinos = _repositorioInquilino.verTodos();
                if (ModelState.IsValid)
                {
                    _repositorio.Modificar(c);
                    TempData["Message"] = "contrato modificado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Message"] = "No se pudo modificar el contrato";
                return View(c);
                
            }
            catch (Exception ex)
            {

                TempData["Message"] = " Error Modificar Contrato";
                return RedirectToAction(nameof(Index));
            }

        }


        [HttpPost]
        public ActionResult Borrar(int id)
        {
            try
            {
                _repositorio.BajaLogica(id);
                TempData["Message"] = " Contrato borrado con exito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "No se pudo borrar el Contrato";

            }
            return RedirectToAction(nameof(Index));

        }

        public ActionResult Detalles(int id)
        {

            var contrato = _repositorio.BuscarId(id);

            if (contrato == null)
            {
                TempData["Message"] = "No se encontro el contrato.";
                return RedirectToAction(nameof(Index));
            }

            return View(contrato);
        }
    


    }
}
