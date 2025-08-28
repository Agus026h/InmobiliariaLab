using devs.Models;
using Microsoft.AspNetCore.Mvc;

namespace devs.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly RepositorioInquilino _repositorio;
        public InquilinoController(RepositorioInquilino repositorio)
        {
            _repositorio = repositorio;
        }

        // GET: InquilinoController
        public ActionResult Index()
        {
            try
            {
                var lista = _repositorio.verTodos();
                return View(lista);
            }
            catch(Exception ex)
            {
                 TempData["Message"] = "Error al cargar los inquilinos: ";
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

                    return NotFound();
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

                    var inquilino = _repositorio.ModificarInquilino(inq);
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
 
    }
    


    



}
