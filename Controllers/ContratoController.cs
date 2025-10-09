using devs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace devs.Controllers
{
    [Authorize]
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
        // EN ContratoController.cs

    public IActionResult Index(
        int pagina = 1,
        int? idInmueble = null,
        string estado = null) 
            {
                var tamanio = 5; 
                pagina = (Math.Max(pagina, 1)); 
                
               
                bool? estadoFiltroBool = null;
                if (!string.IsNullOrEmpty(estado))
                {
                   
                    if (bool.TryParse(estado, out bool parsedEstado))
                    {
                        estadoFiltroBool = parsedEstado;
                    }
                }

              
                var (lista, total) = _repositorio.verTodosPaginado(
                    pagina,
                    tamanio,
                    idInmueble,
                    estadoFiltroBool); 

                
                ViewBag.Pagina = pagina;
                ViewBag.TotalPaginas = (int)Math.Ceiling((double)total / tamanio);

                
                ViewBag.IdInmuebleFiltro = idInmueble;
                ViewBag.EstadoFiltro = estado; 

                
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_TablaContratosPartial", lista);
                }

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

        [Authorize(Policy = "Administrador")]
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
