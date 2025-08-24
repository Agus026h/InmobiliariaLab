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
        public ActionResult Index()
        {

            var lista = _repositorio.verTodos();

            //con esto le paso la lista de propietarios a la vista
            return View(lista);
        }


        public ActionResult Crear()
        {


            return View();
        }


        // POST: Propietario/Crear
        [HttpPost] // para el submit
        [ValidateAntiForgeryToken]

        public ActionResult Crear(Propietario p)
        {
            if (ModelState.IsValid)// se fija si el modelo es valido
                _repositorio.Alta(p);

            return View(p);
        }




    }
    




    
}