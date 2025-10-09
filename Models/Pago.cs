using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace devs.Models;

public class Pago
{
   
    [Display(Name = "ID Pago")]
    public int IdPago { get; set; }

    [Display(Name = "N° Pago")]
    public int NumPago { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de Pago")]
    public DateTime FechaPago { get; set; }

    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal Importe { get; set; }

    [MaxLength(200)]
    public string? Concepto { get; set; }

    [Display(Name = "Estado")]
    public EstadoPago? Estado { get; set; } 

    
    [Display(Name = "Contrato N°")]
    public int IdContrato { get; set; }

    public Contrato? ContratoC { get; set; } 

  

    public int UsuarioCreador { get; set; }

  
    public Usuario? UsuarioCreadorC { get; set; }

    [Display(Name = "Anulado por")]
    public int? UsuarioAnulador { get; set; } // Puede ser nulo

    
    public Usuario? UsuarioAnuladorC { get; set; } 

    public override string ToString()
    {
        return $"Pago N° {NumPago} - Importe: {Importe:C} - Contrato ID: {IdContrato}";
    }
}