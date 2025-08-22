using System;
using System.Data.SqlClient;
using System.Data;

namespace devs.Models;

public class RepositorioPropietario : Conexion
{
    public RepositorioPropietario(IConfiguration configuration) : base(configuration)
    {

    }
    


    public int Alta(Propietario p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = @"INSERT INTO Propietarios 
					(Nombre, Apellido, Dni, Telefono, Email, Clave)
					VALUES (@nombre, @apellido, @dni, @telefono, @email, @clave);
					SELECT LAST_INSERT_ID;";
                    
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.Nombre);

					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@dni", p.Dni);
					command.Parameters.AddWithValue("@telefono", p.Telefono);
					command.Parameters.AddWithValue("@email", p.Email);
                    
					connection.Open();

					res = Convert.ToInt32(command.ExecuteScalar());

					p.IdPropietario = res;
					connection.Close();
				}
			}
			return res;
		}

}
