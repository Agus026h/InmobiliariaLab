using System;
using System.ComponentModel.DataAnnotations;

namespace devs.Models;

public class Propietario
{

    [Key]
    [Display (Name = "Codigo: ")]
    public int IdPropietario { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    public string Apellido { get; set; }

    [Required]
    public string Dni { get; set; }

    [Display(Name = "Telefono")]
    public string Telefono { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    public Boolean Estado { get; set; }
    public IList<Inmueble>? InmueblesPropietario { get; set; }

    public override string ToString()
    {
        var res = $"{Nombre} {Apellido}";

        if (!String.IsNullOrEmpty(Dni))
        {

            res = res + $" {Dni}";

        }
        return res;
    }
    


}
