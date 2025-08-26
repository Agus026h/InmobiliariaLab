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

        public IActionResult Procesar(Propietario p, string accion)
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

        public ActionResult ModificarPropietario(Propietario p)
        {
            _repositorio.ModificarPropietario(p);
            return View();

        }


    }
    




    
}