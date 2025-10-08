using System;
using System.ComponentModel.DataAnnotations;

namespace devs.Models;

public class Inmueble
{

    [Display(Name = "n°")]
    public int IdInmueble { get; set; }
    [Required]
    public string Direccion { get; set; }
    [Required]
    public int Ambientes { get; set; }

    public decimal Precio { get; set; }



    public UsoInmueble Uso { get; set; }

    [Required]
    public int Superficie { get; set; }
    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }
    [Display(Name = "Dueño")]
    public int IdPropietario { get; set; }

    public EstadoInmueble Estado { get; set; }

    public TipoInmueble Tipo { get; set; }

    public Propietario? Duenio { get; set; }
    public IList<Contrato>? ContratosInmueble { get; set; }

    public string? Portada { get; set; }
    
    public IFormFile? PortadaFile{ get; set; }

    public IList<Imagen> Imagenes { get; set; } = new List<Imagen>();

    public override string ToString()
    {
        return $"{Direccion}";
    }
    
}
