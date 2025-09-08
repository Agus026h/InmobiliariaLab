using System;

namespace devs.Models;

public class Contrato
{
    public int IdContrato { get; set; }

    public DateTime FechaInicio { get; set; }
     
    public DateTime? FechaFinOriginal { get; set; }

    public DateTime? FechaFinEfectiva { get; set; }

    public decimal MontoMensual { get; set; }

    public Boolean Estado { get; set; }

    public int IdInquilino { get; set; }

    public int IdInmueble { get; set; }


    //agregar despues cuando este usuario
    //public int idUsuarioCredor { get; set; }

    //public int idUsuarioFinalizador{ get; set;}


    public override string ToString()
    {
        return base.ToString();
    }
}
