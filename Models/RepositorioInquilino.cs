using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;


namespace devs.Models;

public class RepositorioInquilino : Conexion
{
	public RepositorioInquilino(IConfiguration configuration) : base(configuration)
	{

	}



	public int Alta(Inquilino inq)
	{
		int res = -1;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string sql = @"INSERT INTO inquilino 
					(Nombre, Apellido, Dni, Telefono, Email)
					VALUES (@nombre, @apellido, @dni, @telefono, @email);
					SELECT LAST_INSERT_ID();";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.CommandType = CommandType.Text;
				command.Parameters.AddWithValue("@nombre", inq.Nombre);

				command.Parameters.AddWithValue("@apellido", inq.Apellido);
				command.Parameters.AddWithValue("@dni", inq.Dni);
				command.Parameters.AddWithValue("@telefono", inq.Telefono);
				command.Parameters.AddWithValue("@email", inq.Email);

				connection.Open();

				res = Convert.ToInt32(command.ExecuteScalar());

				inq.IdInquilino = res;
				connection.Close();
			}
		}
		return res;
	}
    //metodo para traer todos los inquilinos
	public IList<Inquilino> verTodos()
	{
		IList<Inquilino> listaI = new List<Inquilino>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String sql = @"Select
			idInquilino, nombre, apellido, dni, email, telefono
			From inquilino
			
			";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{

				command.CommandType = CommandType.Text;
				connection.Open();
				var reader = command.ExecuteReader();
				while (reader.Read())
				{
					Inquilino inq = new Inquilino
					{
						IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
						Nombre = reader.GetString("Nombre"),
						Apellido = reader.GetString("Apellido"),
						Dni = reader.GetString("Dni"),
						Telefono = reader.GetString("Telefono"),
						Email = reader.GetString("Email"),



					};
					listaI.Add(inq);

				}
				connection.Close();
			}
			return listaI;
		}

	}


    //metodo para listar todos los activos
    public IList<Inquilino> verActivos()
	{
		IList<Inquilino> listaI = new List<Inquilino>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String sql = @"Select
			idInquilino, nombre, apellido, dni, email, telefono
			From inquilino
			WHERE estado =1
			";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{

				command.CommandType = CommandType.Text;
				connection.Open();
				var reader = command.ExecuteReader();
				while (reader.Read())
				{
					Inquilino inq = new Inquilino
					{
						IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
						Nombre = reader.GetString("Nombre"),
						Apellido = reader.GetString("Apellido"),
						Dni = reader.GetString("Dni"),
						Telefono = reader.GetString("Telefono"),
						Email = reader.GetString("Email"),



					};
					listaI.Add(inq);

				}
				connection.Close();
			}
			return listaI;
		}

	}

	public int Baja(int id)
	{
		int res = -1;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string sql = @"UPDATE  inquilino
					SET estado = 0
					WHERE idInquilino = @idInquilino ;
					";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.CommandType = CommandType.Text;
				command.Parameters.AddWithValue("@idInquilino", id);


				connection.Open();
				// ExecuteNonQuery para los Update
				res = command.ExecuteNonQuery();


				connection.Close();
			}
		}
		return res;
	}



	public int ModificarInquilino(Inquilino inq)
	{
		int res = 1;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			String sql = @"UPDATE inquilino SET
			               nombre = @nombre,
						   apellido = @apellido,
						   dni = @dni,
						   telefono = @telefono,
						   email = @email
						   WHERE idInquilino = @idInquilino";
			
			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.CommandType = CommandType.Text;
				command.Parameters.AddWithValue("@idInquilino", inq.IdInquilino);
				command.Parameters.AddWithValue("@nombre", inq.Nombre);
				command.Parameters.AddWithValue("@apellido", inq.Apellido);
				command.Parameters.AddWithValue("@dni", inq.Dni);
				command.Parameters.AddWithValue("@email", inq.Email);
				command.Parameters.AddWithValue("@telefono", inq.Telefono);

				connection.Open();

				res = command.ExecuteNonQuery();

				inq.IdInquilino = res;
				connection.Close();
			}

			return res;
		}


	 }

	public Inquilino buscarId(int id)
	{
		Inquilino inq = null;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
         string sql = @"SELECT IdInquilino, Nombre, Apellido, Dni, Telefono, Email 
					FROM Inquilino
					WHERE IdInquilino = @idInquilino";
			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@idInquilino", id);
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						inq = new Inquilino
						{
							IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),
							
						};
					}
					connection.Close();
				}
			}
			return inq;


		}


	}


