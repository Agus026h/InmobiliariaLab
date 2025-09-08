using System;
using MySql.Data.MySqlClient;
using System.Data;
namespace devs.Models;

public class RepositorioInmueble : Conexion
{
	public RepositorioInmueble(IConfiguration configuration) : base(configuration)
	{

	}


	public int Alta(Inmueble inmueble)
	{
		int res = -1;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string sql = @"INSERT INTO inmueble (direccion, uso, ambientes, precio, latitud, longitud, estado, tipo, idPropietario)
             VALUES (@direccion, @uso, @ambientes, @precio, @latitud, @longitud, @estado, @tipo, @idPropietario);
             SELECT LAST_INSERT_ID();";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.CommandType = CommandType.Text;
				command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
				command.Parameters.AddWithValue("@uso", inmueble.Uso.ToString());
				command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
				command.Parameters.AddWithValue("@precio", inmueble.Precio);
				command.Parameters.AddWithValue("@latitud", inmueble.Latitud);
				command.Parameters.AddWithValue("@longitud", inmueble.Longitud);
				command.Parameters.AddWithValue("@estado", inmueble.Estado.ToString());
				command.Parameters.AddWithValue("@tipo", inmueble.Tipo.ToString());
				command.Parameters.AddWithValue("@idPropietario", inmueble.IdPropietario);




				connection.Open();

				res = Convert.ToInt32(command.ExecuteScalar());

				inmueble.IdInmueble = res;
				connection.Close();
			}
		}
		return res;
	}


	public IList<Inmueble> ObtenerTodos()
	{
		IList<Inmueble> res = new List<Inmueble>();
		using (var connection = new MySqlConnection(connectionString))
		{
			var sql = @"SELECT idInmueble, direccion, uso, ambientes, precio, latitud, longitud, i.estado,
			p.idPropietario, p.nombre, p.apellido
			FROM Inmueble i INNER JOIN propietario p ON i.idPropietario = p.idPropietario";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.CommandType = CommandType.Text;
				connection.Open();
				var reader = command.ExecuteReader();
				while (reader.Read())
				{
					Inmueble entidad = new Inmueble
					{
						IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
						Direccion = reader[nameof(Inmueble.Direccion)] == DBNull.Value ? "" : reader.GetString(nameof(Inmueble.Direccion)),
						Uso = (UsoInmueble)Enum.Parse(typeof(UsoInmueble), reader.GetString(nameof(Inmueble.Uso))),
						//Portada = reader[nameof(Inmueble.Portada)] == DBNull.Value? null : reader.GetString(nameof(Inmueble.Portada)),
						Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
						Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
						Estado = (EstadoInmueble)Enum.Parse(typeof(EstadoInmueble), reader.GetString(nameof(Inmueble.Estado))),
						Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
						Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
						IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
						Duenio = new Propietario
						{
							IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
							Nombre = reader.GetString(nameof(Propietario.Nombre)),
							Apellido = reader.GetString(nameof(Propietario.Apellido)),

						}
					};
					res.Add(entidad);
				}
				connection.Close();
			}


		}

		return res;
	}



	public int BajaReal(int idInmueble)
	{
		int res = -1;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string sql = @"DELETE FROM inmueble WHERE idInmueble = @idInmueble";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@idInmueble", idInmueble);
				connection.Open();
				res = command.ExecuteNonQuery();




			}

		}

		return res;


	}



	public int BajaLogica(int idInmueble)
	{
		int res = -1;
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string sql = @"UPDATE Inmueble SET estado = 'Suspendido' WHERE idInmueble = @idinmueble";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{

				command.Parameters.AddWithValue("@idInmueble", idInmueble);
				connection.Open();
				res = command.ExecuteNonQuery();
			}

		}
		return res;
	}



	public int ModificarInmueble(Inmueble inmueble)
	{
		int res = -1;

		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string sql = @"UPDATE inmueble SET 
			direccion = @direccion,
			uso = @uso,
			ambientes = @ambientes,
			precio = @precio,
			latitud = @latitud,
			longitud = @longitud,
			estado = @estado,
			tipo = @tipo,
			idPropietario = @idPropietario
			WHERE idInmueble = @idInmueble";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
				command.Parameters.AddWithValue("@uso", inmueble.Uso.ToString());
				command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
				command.Parameters.AddWithValue("@precio", inmueble.Precio);
				command.Parameters.AddWithValue("@latitud", inmueble.Latitud);
				command.Parameters.AddWithValue("@longitud", inmueble.Longitud);
				command.Parameters.AddWithValue("@estado", inmueble.Estado.ToString());
				command.Parameters.AddWithValue("@tipo", inmueble.Tipo.ToString());
				command.Parameters.AddWithValue("@idPropietario", inmueble.IdPropietario);
				command.Parameters.AddWithValue("@idInmueble", inmueble.IdInmueble);

				command.CommandType = CommandType.Text;
				connection.Open();
				res = command.ExecuteNonQuery();


			}




		}
		return res;
	}


    public Inmueble BuscarPorId(int idInmueble)
		{
			Inmueble inmueble = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT idInmueble, direccion, uso, ambientes, precio, latitud, longitud, i.estado, tipo,
			        p.idPropietario, p.nombre, p.apellido
			        FROM Inmueble i INNER JOIN propietario p ON i.idPropietario = p.idPropietario
					WHERE idInmueble=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
				 //si uso add en ves de addWithValue le tengo que especificar el tipo de dato
				  command.Parameters.AddWithValue("@id", idInmueble);
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						inmueble = new Inmueble
						{
							IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
							Direccion = reader[nameof(Inmueble.Direccion)] == DBNull.Value? "" : reader.GetString(nameof(Inmueble.Direccion)),
							Uso  = (UsoInmueble)Enum.Parse(typeof(UsoInmueble), reader.GetString(nameof(Inmueble.Uso))),
							Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
							Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
							Estado = (EstadoInmueble)Enum.Parse(typeof(EstadoInmueble),reader.GetString(nameof(Inmueble.Estado))),
							Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
							Tipo = (TipoInmueble)Enum.Parse(typeof(TipoInmueble),reader.GetString(nameof(Inmueble.Tipo))),
							Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
							IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
							Duenio = new Propietario
							{
								IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
								Nombre = reader.GetString(nameof(Propietario.Nombre)),
								Apellido = reader.GetString(nameof(Propietario.Apellido)),
								
							}
						};
					}
					connection.Close();
				}
			}
			return inmueble;
		}






}

