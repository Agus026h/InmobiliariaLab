using System;

namespace devs.Models;

public abstract class Conexion
{
    protected readonly IConfiguration configuration;
    protected readonly String connectionString;

    protected Conexion(IConfiguration configuration)
    {
        this.configuration = configuration;
        connectionString = configuration["ConnectionStrings:MySql"];

        
    }


}
