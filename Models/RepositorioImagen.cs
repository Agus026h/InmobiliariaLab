using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace devs.Models;

public class RepositorioImagen : Conexion
{
    public RepositorioImagen(IConfiguration configuration) : base(configuration)
    {
    }

    public int Alta(Imagen p)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"INSERT INTO Imagen 
					(idInmueble, Url) 
					VALUES (@idInmueble, @url)";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@idInmueble", p.IdInmueble);
                command.Parameters.AddWithValue("@url", p.Url);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public int Baja(int id)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"DELETE FROM Imagen WHERE idImagen = @id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public int Modificacion(Imagen p)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"
				UPDATE Imagen SET 
					Url=@url
				WHERE idImagen=@idImagen";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@idImagen", p.IdImagen);
                command.Parameters.AddWithValue("@url", p.Url);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public Imagen? ObtenerPorId(int id)
    {
        Imagen? res = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
					SELECT 
						{nameof(Imagen.IdImagen)}, 
						{nameof(Imagen.IdInmueble)}, 
						{nameof(Imagen.Url)} 
					FROM Imagen
					WHERE {nameof(Imagen.IdImagen)}=@id";
            using (MySqlCommand comm = new MySqlCommand(sql, connection))
            {
                comm.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    res = new Imagen();
                    res.IdImagen = reader.GetInt32(nameof(Imagen.IdImagen));
                    res.IdInmueble = reader.GetInt32(nameof(Imagen.IdInmueble));
                    res.Url = reader.GetString(nameof(Imagen.Url));
                }
                connection.Close();
            }
        }
        return res;
    }

    public IList<Imagen> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
    {
        List<Imagen> res = new List<Imagen>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            string sql = @$"
					SELECT 
						{nameof(Imagen.IdImagen)}, 
						{nameof(Imagen.IdInmueble)}, 
						{nameof(Imagen.Url)} 
					FROM Imagen
					ORDER BY =idImagen
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
            using (MySqlCommand comm = new MySqlCommand(sql, conn))
            {
                conn.Open();
                var reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    res.Add(new Imagen
                    {
                        IdImagen = reader.GetInt32(nameof(Imagen.IdImagen)),
                        IdInmueble = reader.GetInt32(nameof(Imagen.IdInmueble)),
                        Url = reader.GetString(nameof(Imagen.Url)),
                    });
                }
                conn.Close();
            }
        }
        return res;
    }

    public int ObtenerCantidad()
    {
        int res = 0;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
					SELECT COUNT(IdImagen)
					FROM Imagen
				";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res;
    }

    public IList<Imagen> BuscarPorInmueble(int IdInmueble)
    {
        List<Imagen> res = new List<Imagen>();
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            string sql = @$"
					SELECT 
						{nameof(Imagen.IdImagen)}, 
						{nameof(Imagen.IdInmueble)}, 
						{nameof(Imagen.Url)} 
					FROM Imagen
					WHERE {nameof(Imagen.IdInmueble)}=@idInmueble";
            using (MySqlCommand comm = new MySqlCommand(sql, conn))
            {
                comm.Parameters.AddWithValue("@idInmueble", IdInmueble);
                conn.Open();
                var reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    res.Add(new Imagen
                    {
                        IdImagen = reader.GetInt32(nameof(Imagen.IdImagen)),
                        IdInmueble = reader.GetInt32(nameof(Imagen.IdInmueble)),
                        Url = reader.GetString(nameof(Imagen.Url)),
                    });
                }
                conn.Close();
            }
        }
        return res;
    }

}
    

