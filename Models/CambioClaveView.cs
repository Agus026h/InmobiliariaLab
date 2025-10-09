using System;
using System.ComponentModel.DataAnnotations;
namespace devs.Models

{
    public class CambioClaveView
    {
        [Required(ErrorMessage = "La contraseña actual es requerida.")] 
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña Actual")] 
        public string ClaveVieja { get; set; } = "";

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [StringLength(50, ErrorMessage = "La clave debe tener entre 6 y 50 caracteres", MinimumLength = 6)] 
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string ClaveNueva { get; set; } = "";

        [Required(ErrorMessage = "Debe repetir la contraseña nueva")]
        [DataType(DataType.Password)]
        [Compare("ClaveNueva", ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Repetir Contraseña Nueva")]
        public string ClaveRepeticion { get; set; } = "";
    }
}