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
            try
            {

                var lista = _repositorio.verTodos();

                //con esto le paso la lista de propietarios a la vista
                return View(lista);
            }
            catch (Exception ex)
            {
                 TempData["Message"] = "Error al cargar los inquilinos: ";
                return View(new List<Propietario>());
            }
        }


        public ActionResult Crear()
        {


            return View();
        }


        
        // POST: Propietario/Crear
        [HttpPost] // para el submit

        public IActionResult Procesar(Propietario p, string accion)
        {
            try
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
            }
            catch (Exception ex)
            {
                ViewBag.message = "Error al realizar la accion";
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
        
        [HttpGet]
        public ActionResult Editar(int id)
        {
        try
        {
        var propietario = _repositorio.buscarId(id);
        if (propietario == null)
        {
            TempData["Message"] = "No se encontro el propietario con ese ID";
            return RedirectToAction(nameof(Index));
        }

        
        return View("Crear", propietario);
        }
        catch (Exception ex)
        {
        TempData["Message"] = "Error al cargar el propietario: " + ex.Message;
        return RedirectToAction(nameof(Index));
        }
}


    }
    




    
}