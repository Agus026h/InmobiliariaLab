using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;


namespace devs.Models;

public class RepositorioPropietario : Conexion
{
	public RepositorioPropietario(IConfiguration configuration) : base(configuration)
	{

	}



	public int Alta(Propietario p)
	{
		int res = -1;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string sql = @"INSERT INTO Propietario 
					(Nombre, Apellido, Dni, Telefono, Email)
					VALUES (@nombre, @apellido, @dni, @telefono, @email);
					SELECT LAST_INSERT_ID();";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
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

	public IList<Propietario> verTodos()
	{
		IList<Propietario> listaP = new List<Propietario>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String sql = @"Select
			IdPropietario, nombre, apellido, dni, email, telefono
			From Propietario
			";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{

				command.CommandType = CommandType.Text;
				connection.Open();
				var reader = command.ExecuteReader();
				while (reader.Read())
				{
					Propietario p = new Propietario
					{
						IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
						Nombre = reader.GetString("Nombre"),
						Apellido = reader.GetString("Apellido"),
						Dni = reader.GetString("Dni"),
						Telefono = reader.GetString("Telefono"),
						Email = reader.GetString("Telefono"),



					};
					listaP.Add(p);

				}
				connection.Close();
			}
			return listaP;
		}

	}

  
  
  public int Baja(Propietario p)
	{
		int res = -1;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string sql = @"UPDATE  Propietario 
					SET estado = 0
					WHERE idPropietario = @idPropietario ;
					";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.CommandType = CommandType.Text;
				command.Parameters.AddWithValue("idPropietario", p.IdPropietario);

				
				connection.Open();

				res = Convert.ToInt32(command.ExecuteScalar());

				p.IdPropietario = res;
				connection.Close();
			}
		}
		return res;
	}

}
