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
						Email = reader.GetString("Email"),



					};
					listaP.Add(p);

				}
				connection.Close();
			}
			return listaP;
		}

	}



	public int Baja(int id)
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
				command.Parameters.AddWithValue("@idPropietario", id);


				connection.Open();

				res = command.ExecuteNonQuery();


				connection.Close();
			}
		}
		return res;
	}



	public int ModificarPropietario(Propietario p)
	{
		int res = 1;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String sql = @"UPDATE propietario SET
			               nombre = @nombre,
						   apellido = @apellido,
						   dni = @dni,
						   telefono = @telefono,
						   estado = @estado,
						   email = @email
						   WHERE idPropietario = @idPropietario";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.CommandType = CommandType.Text;
				command.Parameters.AddWithValue("@idPropietario", p.IdPropietario);
				command.Parameters.AddWithValue("@nombre", p.Nombre);
				command.Parameters.AddWithValue("@apellido", p.Apellido);
				command.Parameters.AddWithValue("@estado", p.Estado);
				command.Parameters.AddWithValue("@dni", p.Dni);
				command.Parameters.AddWithValue("@email", p.Email);
				command.Parameters.AddWithValue("@telefono", p.Telefono);

				connection.Open();

				res = command.ExecuteNonQuery();

				p.IdPropietario = res;
				connection.Close();
			}

			return res;
		}


	}

	public Propietario buscarId(int id)
	{
		Propietario p = null;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string sql = @"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Estado 
					FROM Propietario
					WHERE IdPropietario = @idPropietario";
			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@idPropietario", id);
				command.CommandType = CommandType.Text;
				connection.Open();
				var reader = command.ExecuteReader();
				if (reader.Read())
				{
					p = new Propietario
					{
						IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
						Nombre = reader.GetString("Nombre"),
						Apellido = reader.GetString("Apellido"),
						Dni = reader.GetString("Dni"),
						Estado = reader.GetBoolean("Estado"),
						Telefono = reader.GetString("Telefono"),
						Email = reader.GetString("Email"),

					};
				}
				connection.Close();
			}
		}
		return p;


	}


	public IList<Propietario> buscarPorNombre(string nombre)
	{
		List<Propietario> res = new List<Propietario>();
		Propietario? p = null;
		nombre = "%" + nombre + "%";

		using (var connection = new MySqlConnection(connectionString))
		{
			string sql = @"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email, Estado 
					FROM Propietario
					WHERE nombre like @nombre OR apellido = @nombre";
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@nombre", nombre);
				command.CommandType = CommandType.Text;
				connection.Open();
				var reader = command.ExecuteReader();
				while (reader.Read())
				{
					p = new Propietario
					{
						IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
						Nombre = reader.GetString("Nombre"),
						Apellido = reader.GetString("Apellido"),
						Dni = reader.GetString("Dni"),
						Telefono = reader.GetString("Telefono"),
						Email = reader.GetString("Email"),
						//Clave = reader.GetString("Clave"),
					};
					res.Add(p);
				}
				connection.Close();
			}
			return res;
		}
	}






	public int BajaReal(int id)
	{
		int res = -1;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string sql = @"Delete propietario 
                           WHERE idPropietario = @idPropietario";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.CommandType = CommandType.Text;
				command.Parameters.AddWithValue("@idPropietario", id);
				connection.Open();
				res = command.ExecuteNonQuery();
			}
		}
		return res;
	}


	public (IList<Propietario> Lista, int totalRegistro) verTodosPaginado(int paginaNro = 1, int paginaTam = 10, string dni = null, bool? estado = null)
	{
		IList<Propietario> listaP = new List<Propietario>();
		int totalRegistro = 0;
		string filtro = "";

		
		if (!string.IsNullOrEmpty(dni))
		{
			filtro += $" WHERE Dni LIKE '%{dni}%'";
		}

		
		if (estado.HasValue)
		{
			string clausula = filtro.Length > 0 ? " AND " : " WHERE ";
			string estadoSql = estado.Value ? "1" : "0";
			filtro += $"{clausula} estado = {estadoSql}"; 
		}

		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			connection.Open();
			string contador = $"SELECT COUNT(*) FROM Propietario {filtro}";
			using (var countCommand = new MySqlCommand(contador, connection))
         {
            countCommand.CommandType = CommandType.Text;
            
            totalRegistro = Convert.ToInt32(countCommand.ExecuteScalar());
         }


			String sql = @$"SELECT
            IdPropietario, Nombre, Apellido, Dni, Email, Telefono, Estado
            FROM Propietario
            {filtro}
            LIMIT {paginaTam} OFFSET {(paginaNro - 1) * paginaTam}  
        ";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.CommandType = CommandType.Text;
                var reader = command.ExecuteReader();
				while (reader.Read())
				{
					Propietario p = new Propietario
					{
						IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
                        Nombre = reader.GetString(nameof(Propietario.Nombre)),
                        Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        Dni = reader.GetString(nameof(Propietario.Dni)),
                        Email = reader.GetString(nameof(Propietario.Email)),
                        Telefono = reader.GetString(nameof(Propietario.Telefono)),
						Estado = reader.GetBoolean(nameof(Propietario.Estado))
					};
					listaP.Add(p);
				}
				connection.Close();
			}
			return (listaP, totalRegistro);
		}
	}



}


