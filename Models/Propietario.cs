using System;
using System.ComponentModel.DataAnnotations;

namespace devs.Models;

public class Propietario
{

    [Key]
    [Display (Name = "Codigo: ")]
    public int IdPropietario { get; set; }

    [Required]
    public String Nombre { get; set; }

    [Required]
    public String Apellido { get; set; }

    [Required]
    public String Dni { get; set; }

    [Display(Name = "Telefono")]
    public String Telefono { get; set; }

    [Required, EmailAddress]
    public String Email { get; set; }

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
