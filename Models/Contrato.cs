using System;
using System.ComponentModel.DataAnnotations;

namespace devs.Models;

public class Contrato
{
    public int IdContrato { get; set; }

    public DateTime FechaInicio { get; set; }
     
    public DateTime? FechaFinOriginal { get; set; }

    public DateTime? FechaFinEfectiva { get; set; }

    public decimal MontoMensual { get; set; }

    public Boolean Estado { get; set; }
    [Display(Name = "Inquilino")]
    public int IdInquilino { get; set; }

    public Inquilino? InquilinoC { get; set; }
    [Display(Name = "Inmueble")]
    public int IdInmueble { get; set; }

    public Inmueble? InmuebleC {get; set;}


    
    public int? IdUsuarioCredor { get; set; }

    public int? IdUsuarioFinalizador{ get; set;}


    public override string ToString()
    {
        return base.ToString();
    }
}
