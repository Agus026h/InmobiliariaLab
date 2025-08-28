using System;
using System.ComponentModel.DataAnnotations;

namespace devs.Models;

public class Inquilino
{
    [Key]
    [Display(Name = "Codigo: ")]
    public int IdInquilino { get; set; }

    [Required]
    public String Nombre { get; set; }
    
    [Required]
    public String Apellido { get; set; }

    [Required]
    public String Dni { get; set; }

    [Required, EmailAddress]
    public String Email { get; set; }

    [Required]
    public String Telefono { get; set; }
    
    public override string ToString()
    {
        var res = $"{Nombre} {Apellido}";

        if (!String.IsNullOrEmpty(Dni))
        {

            res = res + $"{Dni}";

        }
        return res;
    }

}
