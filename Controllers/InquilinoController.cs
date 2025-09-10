using devs.Models;
using Microsoft.AspNetCore.Mvc;

namespace devs.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino _repositorio;
        private readonly RepositorioContrato _repositorioContrato;
        public InquilinoController(RepositorioInquilino repositorio, RepositorioContrato repositorioContrato)
        {
            _repositorio = repositorio;
            _repositorioContrato = repositorioContrato;
        }

        // GET: InquilinoController

        public ActionResult Index(bool? mostrarInactivos)
        {
            try
            {
                IList<Inquilino> lista;

                if (mostrarInactivos.GetValueOrDefault(false))
                {

                    lista = _repositorio.verTodos();
                }
                else
                {

                    lista = _repositorio.verActivos();
                }

                ViewBag.MostrarInactivos = mostrarInactivos.GetValueOrDefault(false);
                return View(lista);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error al cargar los inquilinos.";
                return View(new List<Inquilino>());
            }
        }

        public ActionResult Crear()
        {
            return View();


        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            try
            {


                var inquilino = _repositorio.buscarId(id);
                if (inquilino == null)
                {

                    TempData["Message"] = "No se encontro al inquilino";
                    return RedirectToAction(nameof(Index));
                }

                return View(inquilino);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "No se encontro al inquilino";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modificar(Inquilino inq)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _repositorio.ModificarInquilino(inq);
                    TempData["Message"] = "Inquilino modificado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                return View(inq);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "No se pudo modificar inquilino";
                return View(inq);

            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Inquilino inq)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    _repositorio.Alta(inq);
                    TempData["Message"] = "Inquilino creado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                return View(inq);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "error al crear el inquilino";
                return View(inq);
            }
        }


        [HttpPost]
        public ActionResult Borrar(int id)
        {
            try
            {
                _repositorio.Baja(id);
                TempData["Message"] = " Inquilino borrado con exito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "No se pudo borrar el inquilino";


            }
            return RedirectToAction(nameof(Index));



        }


        [HttpGet]
        public ActionResult Detalles(int id)
        {
            try {
                var inquilino = _repositorio.buscarId(id);

                var contratos = _repositorioContrato.ObtenerContratosPorInquilino(id);

                inquilino.ContratosInquilino = contratos;

                return View(inquilino);
            }catch (Exception ex)
            {

                TempData["Message"] = " Error al cargar detalles";
                return RedirectToAction(nameof(Index));
            }


        }

       



    }
}
