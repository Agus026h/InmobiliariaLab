using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace devs.Models;

public class RepositorioUsuario : Conexion
{
    public RepositorioUsuario(IConfiguration configuration) : base(configuration)
    {

    }

    public int Alta(Usuario u)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"INSERT INTO Usuario 
					(Nombre, Apellido, Avatar, Email, Clave, Rol) 
					VALUES (@nombre, @apellido, @avatar, @email, @clave, @rol);
					SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", u.Nombre);
                command.Parameters.AddWithValue("@apellido", u.Apellido);
                if (String.IsNullOrEmpty(u.Avatar))
                    command.Parameters.AddWithValue("@avatar", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@avatar", u.Avatar);
                command.Parameters.AddWithValue("@email", u.Email);
                command.Parameters.AddWithValue("@clave", u.Clave);
                command.Parameters.AddWithValue("@rol", u.Rol);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                u.IdUsuario = res;
                connection.Close();
            }
        }
        return res;
    }
    public int Baja(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = "DELETE FROM Usuario WHERE idUsuario = @id";
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
    public int Modificacion(Usuario e)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE Usuario
					SET Nombre=@nombre, Apellido=@apellido, Avatar=@avatar, Email=@email, Clave=@clave, Rol=@rol
					WHERE idUsuario = @id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", e.Nombre);
                command.Parameters.AddWithValue("@apellido", e.Apellido);
                command.Parameters.AddWithValue("@avatar", String.IsNullOrEmpty(e.Avatar) ? DBNull.Value : e.Avatar);
                command.Parameters.AddWithValue("@email", e.Email);
                command.Parameters.AddWithValue("@clave", e.Clave);
                command.Parameters.AddWithValue("@rol", e.Rol);
                command.Parameters.AddWithValue("@id", e.IdUsuario);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public IList<Usuario> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
    {
        IList<Usuario> res = new List<Usuario>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
					SELECT idUsuario, Nombre, Apellido, Avatar, Email, Clave, Rol
					FROM Usuario
					ORDER BY idUsuario
					";// completar
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Usuario e = new Usuario
                    {
                        IdUsuario = reader.GetInt32("idUsuario"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Avatar = reader["Avatar"] is DBNull ? null : reader.GetString("Avatar"),
                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        Rol = (Roles)Enum.Parse(typeof(Roles), reader.GetString(nameof(Usuario.Rol))),
                    };
                    res.Add(e);
                }
                connection.Close();
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
					SELECT COUNT({nameof(Usuario.IdUsuario)})
					FROM Usuario
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



    public Usuario? ObtenerPorEmail(string email)
    {
        Usuario? e = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT
					idUsuario, Nombre, Apellido, Avatar, Email, Clave, Rol FROM Usuario
					WHERE Email=@email";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    e = new Usuario
                    {
                        IdUsuario = reader.GetInt32("idUsuario"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Avatar = reader["Avatar"] is DBNull ? null : reader.GetString("Avatar"),
                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        Rol = (Roles)Enum.Parse(typeof(Roles), reader.GetString(nameof(Usuario.Rol))),
                    };
                }
                connection.Close();
            }
        }
        return e;
    }

    public Usuario? ObtenerPorId(int id)
    {
        Usuario? e = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT 
					idUsuario, Nombre, Apellido, Avatar, Email, Clave, Rol 
					FROM Usuario
					WHERE idUsuario=@id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    e = new Usuario
                    {
                        IdUsuario = reader.GetInt32("idUsuario"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Avatar = reader["Avatar"] is DBNull ? null : reader.GetString("Avatar"),
                        Email = reader.GetString("Email"),
                        Clave = reader.GetString("Clave"),
                        Rol = (Roles)Enum.Parse(typeof(Roles), reader.GetString(nameof(Usuario.Rol))),
                    };
                }
                connection.Close();
            }
        }
        return e;
    }
        
}

