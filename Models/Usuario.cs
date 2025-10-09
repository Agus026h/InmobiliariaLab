using System;
using System.ComponentModel.DataAnnotations;

namespace devs.Models;

public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }
    [Required]
    public string Nombre { get; set; } = "";
    [Required]
    public string Apellido { get; set; } = "";
    [Required, EmailAddress]
    public string Email { get; set; } = "";
    [Required, DataType(DataType.Password)]
    public string Clave { get; set; } = "";
    public string? Avatar { get; set; }

    public IFormFile? AvatarFile { get; set; }
 		
    public Roles Rol { get; set; }       
}
