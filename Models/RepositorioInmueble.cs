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
			var sql = @"SELECT idInmueble, direccion, uso, ambientes, precio, latitud, longitud, portada, i.estado,
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
						Portada = reader[nameof(Inmueble.Portada)] == DBNull.Value? null : reader.GetString(nameof(Inmueble.Portada)),
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
					SELECT idInmueble, direccion, uso, ambientes, precio, latitud, longitud, portada, i.estado, tipo,
			        p.idPropietario, p.nombre, p.apellido, p.email
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
						Direccion = reader[nameof(Inmueble.Direccion)] == DBNull.Value ? "" : reader.GetString(nameof(Inmueble.Direccion)),
						Uso = (UsoInmueble)Enum.Parse(typeof(UsoInmueble), reader.GetString(nameof(Inmueble.Uso))),
						Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
						Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
						Portada = reader[nameof(Inmueble.Portada)] == DBNull.Value? null : reader.GetString(nameof(Inmueble.Portada)),
						Estado = (EstadoInmueble)Enum.Parse(typeof(EstadoInmueble), reader.GetString(nameof(Inmueble.Estado))),
						Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
						Tipo = (TipoInmueble)Enum.Parse(typeof(TipoInmueble), reader.GetString(nameof(Inmueble.Tipo))),
						Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
						IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
						Duenio = new Propietario
						{
							IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
							Nombre = reader.GetString(nameof(Propietario.Nombre)),
							Apellido = reader.GetString(nameof(Propietario.Apellido)),
							Email = reader.GetString(nameof(Propietario.Email))

						}
					};
				}
				connection.Close();
			}
		}
		return inmueble;
	}
	public IList<Inmueble> ObtenerPorPropietario(int idPropietario)
	{
		IList<Inmueble> lista = new List<Inmueble>();
		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			string sql = @"SELECT i.idInmueble, i.direccion, i.uso, i.ambientes, i.precio, i.latitud, i.longitud, i.estado, i.tipo,
                p.idPropietario, p.nombre, p.apellido
            FROM Inmueble i INNER JOIN propietario p ON i.idPropietario = p.idPropietario
            WHERE i.idPropietario = @idPropietario";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@idPropietario", idPropietario);
				command.CommandType = CommandType.Text;
				connection.Open();
				var reader = command.ExecuteReader();

				while (reader.Read())
				{
					Inmueble inmueble = new Inmueble
					{
						IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
						Direccion = reader[nameof(Inmueble.Direccion)] == DBNull.Value ? "" : reader.GetString(nameof(Inmueble.Direccion)),
						Uso = (UsoInmueble)Enum.Parse(typeof(UsoInmueble), reader.GetString(nameof(Inmueble.Uso))),
						Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
						Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
						Estado = (EstadoInmueble)Enum.Parse(typeof(EstadoInmueble), reader.GetString(nameof(Inmueble.Estado))),
						Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
						Tipo = (TipoInmueble)Enum.Parse(typeof(TipoInmueble), reader.GetString(nameof(Inmueble.Tipo))),
						Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
						IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
						Duenio = new Propietario
						{
							IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
							Nombre = reader.GetString(nameof(Propietario.Nombre)),
							Apellido = reader.GetString(nameof(Propietario.Apellido)),
						}
					};
					lista.Add(inmueble);
				}
				connection.Close();
			}
		}
		return lista;
	}

	public IList<Inmueble> buscarPorNombre(string direccion)
	{
		List<Inmueble> res = new List<Inmueble>();
		Inmueble? inm = null;
		direccion = "%" + direccion + "%";

		using (var connection = new MySqlConnection(connectionString))
		{
			string sql = @"SELECT IdInmueble, direccion, uso, precio
					FROM Inmueble
					WHERE direccion like @direccion";
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@direccion", direccion);
				command.CommandType = CommandType.Text;
				connection.Open();
				var reader = command.ExecuteReader();
				while (reader.Read())
				{
					inm = new Inmueble
					{
						IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
						Direccion = reader.GetString("direccion"),
						Uso = (UsoInmueble)Enum.Parse(typeof(UsoInmueble), reader.GetString(nameof(Inmueble.Uso))),
						Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
						//Clave = reader.GetString("Clave"),
					};
					res.Add(inm);
				}
				connection.Close();
			}
			return res;
		}
	}


	public int ModificarPortada(int id, string url)
	{
		int res = -1;
		using (var connection = new MySqlConnection(connectionString))
		{
			string sql = @"
					UPDATE Inmueble SET
					Portada=@portada
					WHERE idInmueble = @id";
			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@portada", String.IsNullOrEmpty(url) ? DBNull.Value : url);
				command.Parameters.AddWithValue("@id", id);
				command.CommandType = CommandType.Text;
				connection.Open();
				res = command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return res;
	}
	
	// paginado con filtros
	public (IList<Inmueble> Lista, int totalRegistro) verTodosPaginado(
		int paginaNro = 1, int paginaTam = 5,
		string direccion = null, int? uso = null, int? estado = null, string nombrePropietario = null)
	{
		IList<Inmueble> listaI = new List<Inmueble>();
		int totalRegistro = 0;
		string filtro = "";
		string whereClausula = "";



        //probando otra forma de armar el filtro
		Action<string> agregarFiltro = (condicion) =>
		{
			if (whereClausula.Length == 0)
			{
				whereClausula = " WHERE " + condicion;
			}
			else
			{
				whereClausula += " AND " + condicion;
			}
		};


		if (!string.IsNullOrEmpty(direccion))
		{

			agregarFiltro($"i.Direccion LIKE '%{direccion}%'");
		}
		if (uso.HasValue)
		{
			agregarFiltro($"i.Uso = {uso.Value + 1}");
		}
		if (estado.HasValue)
		{
			agregarFiltro($"i.Estado = {estado.Value + 1}");
		}
		if (!string.IsNullOrEmpty(nombrePropietario))
		{
			agregarFiltro($"(p.Nombre LIKE '%{nombrePropietario}%' OR p.Apellido LIKE '%{nombrePropietario}%')");
		}

		filtro = whereClausula;

		using (MySqlConnection connection = new MySqlConnection(connectionString))
		{
			connection.Open();

			
			string contador = $"SELECT COUNT(*) FROM Inmueble i JOIN Propietario p ON i.IdPropietario = p.IdPropietario {filtro}";

			using (var countCommand = new MySqlCommand(contador, connection))
			{
				countCommand.CommandType = CommandType.Text;
				totalRegistro = Convert.ToInt32(countCommand.ExecuteScalar());
			}

			
			String sql = @$"SELECT
            i.IdInmueble, i.Direccion, i.Uso, i.Ambientes, i.Precio, i.IdPropietario, i.Estado, i.Portada, 
            p.Nombre AS PropietarioNombre, p.Apellido AS PropietarioApellido
            FROM Inmueble i 
            JOIN Propietario p ON i.IdPropietario = p.IdPropietario
            {filtro}
            ORDER BY i.IdInmueble
            LIMIT {paginaTam} OFFSET {(paginaNro - 1) * paginaTam}  
        ";

			using (MySqlCommand command = new MySqlCommand(sql, connection))
			{
				command.CommandType = CommandType.Text;
				var reader = command.ExecuteReader();

				while (reader.Read())
				{
					Inmueble i = new Inmueble
					{
						IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
						Direccion = reader.GetString(nameof(Inmueble.Direccion)),
						
						Uso = (UsoInmueble)Enum.Parse(typeof(UsoInmueble), reader.GetString(nameof(Inmueble.Uso))),
						Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
						Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
						IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
						 Estado = (EstadoInmueble)Enum.Parse(typeof(EstadoInmueble), reader.GetString(nameof(Inmueble.Estado)), true),
						
						Portada = reader.IsDBNull(reader.GetOrdinal(nameof(Inmueble.Portada))) ? null : reader.GetString(nameof(Inmueble.Portada)),

						
						Duenio = new Propietario
						{
							Nombre = reader.GetString("PropietarioNombre"),
							Apellido = reader.GetString("PropietarioApellido")
						}
					};
					listaI.Add(i);
				}
				connection.Close();
			}
			return (listaI, totalRegistro);
		}
	}


}





