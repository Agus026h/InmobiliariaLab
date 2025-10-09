namespace devs.Models
{
    public enum UsoInmueble
    {
        Residencial,
        Comercial
    }

    public enum EstadoInmueble
    {
        Disponible,
        Alquilado,
        Vendido,
        Suspendido
    }

    public enum TipoInmueble
    {
        Local,
        Deposito,
        Casa,
        Departamento

    }

    public enum Roles
    {
        Administrador,
        User
    }
    
    public enum EstadoPago
    {
        
        Pendiente = 0,
        Pagado = 1,
        Anulado = 2
    }
}