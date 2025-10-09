using devs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace devs.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private readonly RepositorioPago _repositorioPago;
        private readonly RepositorioContrato _repositorioContrato;
        private readonly RepositorioInmueble _repositorioInmueble;

        public PagoController(
            RepositorioPago repositorioPago,
            RepositorioContrato repositorioContrato,
            RepositorioInmueble repositorioInmueble)
        {
            _repositorioPago = repositorioPago;
            _repositorioContrato = repositorioContrato;
            _repositorioInmueble = repositorioInmueble;
        }



        // GET: PagoController
        public IActionResult Index(
            int pagina = 1,
            int? contratoId = null,
            string estado = null,
            string direccionInmueble = null)
        {
            var tamanio = 5;
            pagina = (Math.Max(pagina, 1));




            var (lista, total) = _repositorioPago.verTodosPaginado(
                pagina,
                tamanio,
                contratoId,
                estado,
                direccionInmueble);


            ViewBag.Pagina = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / tamanio);

            ViewBag.ContratoIdFiltro = contratoId;
            ViewBag.EstadoFiltro = estado;
            ViewBag.DireccionInmuebleFiltro = direccionInmueble;



            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_TablaPagosPartial", lista);
            }

            return View(lista);
        }




        // GET: PagoController/Crear
        public ActionResult Crear(int? contratoId)
        {

            ViewBag.Contratos = _repositorioContrato.VerTodos();



            return View(new Pago());
        }

        // POST: PagoController/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Pago p)
        {
            ViewBag.Contratos = _repositorioContrato.VerTodos();
            if (p.IdContrato <= 0)
            {
                ModelState.AddModelError("IdContrato", "Debe seleccionar un contrato valido.");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Establecer Estado y Usuario Creador


                    p.UsuarioCreador = int.Parse(User.FindFirstValue("IdUsuario"));


                    _repositorioPago.Alta(p);
                    TempData["Message"] = $"Pago N° {p.NumPago} registrado correctamente para el Contrato {p.IdContrato}.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {

                TempData["Message"] = "Error al registrar el pago: " + ex.Message;
            }


            if (p.IdContrato > 0)
            {
                var contrato = _repositorioContrato.BuscarId(p.IdContrato);
                if (contrato != null)
                {
                    contrato.InmuebleC = _repositorioInmueble.BuscarPorId(contrato.IdInmueble);
                    ViewBag.ContratoIdInicial = p.IdContrato;
                    ViewBag.ContratoInicial = contrato;
                }
            }

            return View(p);
        }



        // GET: PagoController/Detalles/5
        public ActionResult Detalles(int idPago)
        {
            var pago = _repositorioPago.BuscarId(idPago);

            if (pago == null)
            {
                TempData["Message"] = "No se encontro el pago.";
                return RedirectToAction(nameof(Index));
            }

            return View(pago);
        }

        // GET: PagoController/Modificar/5
        public ActionResult Modificar(int idPago)
        {
            var pago = _repositorioPago.BuscarId(idPago);
            if (pago == null)
            {
                TempData["Message"] = "Pago no encontrado";
                return RedirectToAction(nameof(Index));
            }
            return View(pago);
        }

        // POST: PagoController/Modificar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modificar(Pago p)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _repositorioPago.Modificar(p);
                    TempData["Message"] = "Pago modificado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                return View(p);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error al modificar el pago: " + ex.Message;
                return View(p);
            }
        }

        // POST: PagoController/BajaLogica/5 (Anulacion)
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BajaLogica(int id, int idUsuarioAnulador)
        {
            try
            {
                int usuarioAnuladorId = int.Parse(User.FindFirstValue("IdUsuario"));

                //Ejecutar la baja logica (Anular el pago)

                _repositorioPago.BajaLogica(id, usuarioAnuladorId);

                TempData["Message"] = "El pago fue ANULADO exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error al anular el pago: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public ActionResult Pagar(int id)
        {


            int resultado = _repositorioPago.MarcarComoPagado(id);

            if (resultado > 0)
            {
                TempData["Message"] = $"El Pago N° {id} ha sido marcado como PAGADO ";
            }
            else
            {
                TempData["Message"] = $"ERROR: No se pudo marcar el Pago N° {id} como PAGADO";
            }

            return RedirectToAction(nameof(Index));
        }
    }   
     
}