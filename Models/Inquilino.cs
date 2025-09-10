using System;
using System.ComponentModel.DataAnnotations;

namespace devs.Models;

public class Inquilino
{
    [Key]
    [Display(Name = "Codigo: ")]
    public int IdInquilino { get; set; }

    [Required]
    public string Nombre { get; set; }
    
    [Required]
    public string Apellido { get; set; }

    [Required]
    public string Dni { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Telefono { get; set; }

    public Boolean Estado { get; set; }

    public IList<Contrato>? ContratosInquilino{ get; set; }
    
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
