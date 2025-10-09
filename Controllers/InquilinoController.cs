using devs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace devs.Controllers
{
    [Authorize]
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino _repositorio;
        private readonly RepositorioContrato _repositorioContrato;
        public InquilinoController(RepositorioInquilino repositorio, RepositorioContrato repositorioContrato)
        {
            _repositorio = repositorio;
            _repositorioContrato = repositorioContrato;
        }



        public ActionResult Index(int pagina = 1, string dni = null, string estado = null)
        {
            try
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

                //si es ajax 
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    //devuelve vista parcial
                    return PartialView("_TablaInquilinosPartial", lista);

                }

                return View(lista);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error al cargar los inquilinos." + ex;
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

        [Authorize(Policy = "Administrador")]
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
            try
            {
                var inquilino = _repositorio.buscarId(id);

                var contratos = _repositorioContrato.ObtenerContratosPorInquilino(id);

                inquilino.ContratosInquilino = contratos;

                return View(inquilino);
            }
            catch (Exception ex)
            {

                TempData["Message"] = " Error al cargar detalles";
                return RedirectToAction(nameof(Index));
            }


        }

        [Route("[controller]/Buscar/{q}")]
        public IActionResult Buscar(string q)
        {
            try
            {
                var res = _repositorio.buscarPorNombre(q);
                return Json(new { datos = res });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }



    }
}
