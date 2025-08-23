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
    }
}