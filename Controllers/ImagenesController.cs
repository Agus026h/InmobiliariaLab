using System;
using Microsoft.AspNetCore.Mvc;
using devs.Models;
using Microsoft.AspNetCore.Authorization;

namespace devs.Controllers
{
    [Authorize]
    public class ImagenesController : Controller
    {
        private readonly RepositorioImagen repositorio;

        public ImagenesController(RepositorioImagen repositorio)
        {
            this.repositorio = repositorio;
        }

        [HttpPost]
        public async Task<IActionResult> Alta(int id, List<IFormFile> imagenes, [FromServices] IWebHostEnvironment environment)
        {
            if (imagenes == null || imagenes.Count == 0)
                return BadRequest("No se recibieron archivos.");
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, "Inmuebles");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, id.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (var file in imagenes)
            {
                if (file.Length > 0)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                    var rutaArchivo = Path.Combine(path, nombreArchivo);

                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    Imagen imagen = new Imagen
                    {
                        IdInmueble = id,
                        Url = $"/Uploads/Inmuebles/{id}/{nombreArchivo}",
                    };
                    repositorio.Alta(imagen);
                }
            }
            return Ok(repositorio.BuscarPorInmueble(id));
        }

        [HttpPost]
        
        public ActionResult Eliminar(int id, [FromServices] IWebHostEnvironment environment)
        {
            try
            {

                var entidad = repositorio.ObtenerPorId(id);
               // borrado fisico
                string urlRelativa = entidad.Url.TrimStart('~', '/');
                string rutaFisica = Path.Combine(environment.WebRootPath, urlRelativa);
                if (System.IO.File.Exists(rutaFisica))
                {
                    System.IO.File.Delete(rutaFisica);
                    
                }
                repositorio.Baja(id);
                return Ok(repositorio.BuscarPorInmueble(entidad.IdInmueble));
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

    }
}


