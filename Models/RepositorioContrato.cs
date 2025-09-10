using System;
using MySql.Data.MySqlClient;
using System.Data;


namespace devs.Models;

public class RepositorioContrato : Conexion
{

    public RepositorioContrato(IConfiguration configuration) : base(configuration)
    {
    }


    public int Alta(Contrato c)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"INSERT INTO contrato 
                             (FechaInicio, FechaFinOriginal, FechaFinEfectiva, MontoMensual, Estado, IdInquilino, IdInmueble)
                             VALUES (@fechaInicio, @fechaFinOriginal, @fechaFinEfectiva, @montoMensual, @estado, @idInquilino, @idInmueble);
                             SELECT LAST_INSERT_ID();";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@fechaInicio", c.FechaInicio);
                command.Parameters.AddWithValue("@fechaFinOriginal", c.FechaFinOriginal);
                command.Parameters.AddWithValue("@fechaFinEfectiva", c.FechaFinEfectiva);
                command.Parameters.AddWithValue("@montoMensual", c.MontoMensual);
                command.Parameters.AddWithValue("@estado", c.Estado);
                command.Parameters.AddWithValue("@idInquilino", c.IdInquilino);
                command.Parameters.AddWithValue("@idInmueble", c.IdInmueble);

                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                c.IdContrato = res;
            }
        }
        return res;
    }

    public IList<Contrato> VerTodos()
    {
        IList<Contrato> lista = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT c.IdContrato, c.FechaInicio, c.FechaFinOriginal, c.FechaFinEfectiva, c.MontoMensual, c.Estado, c.IdInquilino, c.IdInmueble,
                          i.nombre, i.apellido, inm.direccion
                          FROM contrato c JOIN inquilino i ON c.IdInquilino = i.idInquilino
                          JOIN inmueble inm ON c.idInmueble = inm.idInmueble
                          ";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Contrato
                    {
                        IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                        FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                        FechaFinOriginal = reader.IsDBNull("FechaFinOriginal") ? (DateTime?)null : reader.GetDateTime("FechaFinOriginal"),
                        FechaFinEfectiva = reader.IsDBNull("FechaFinEfectiva") ? (DateTime?)null : reader.GetDateTime("FechaFinEfectiva"),
                        MontoMensual = reader.GetDecimal(nameof(Contrato.MontoMensual)),
                        Estado = reader.IsDBNull(nameof(Contrato.Estado)) ? false : Convert.ToBoolean(reader[nameof(Contrato.Estado)]),
                        IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                        IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                        InquilinoC = new Inquilino
                        {
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido))
                        },
                        InmuebleC = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion))
                        }

                    });
                }
            }
        }
        return lista;
    }



    public Contrato BuscarId(int id)
    {
        Contrato contrato = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT c.IdContrato, c.FechaInicio, c.FechaFinOriginal, c.FechaFinEfectiva, c.MontoMensual, c.Estado, c.IdInquilino, c.IdInmueble,
                            i.nombre, i.apellido, inm.direccion
                             FROM contrato c 
                             JOIN inquilino i ON i.idInquilino = c.idInquilino
                             JOIN inmueble inm ON c.idInmueble = inm.idInmueble
                             WHERE IdContrato = @id";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    contrato = new Contrato
                    {
                        IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                        FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                        FechaFinOriginal = reader.IsDBNull("FechaFinOriginal") ? (DateTime?)null : reader.GetDateTime("FechaFinOriginal"),
                        FechaFinEfectiva = reader.IsDBNull("FechaFinEfectiva") ? (DateTime?)null : reader.GetDateTime("FechaFinEfectiva"),
                        MontoMensual = reader.GetDecimal(nameof(Contrato.MontoMensual)),
                        Estado = reader.IsDBNull(nameof(Contrato.Estado)) ? false : Convert.ToBoolean(reader[nameof(Contrato.Estado)]),
                        IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                        IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                        InquilinoC = new Inquilino
                        {
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido))
                        },
                        InmuebleC = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion))
                        }
                    };
                }
            }
        }
        return contrato;
    }

    public int Modificar(Contrato c)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE contrato SET
                             FechaInicio = @fechaInicio,
                             FechaFinOriginal = @fechaFinOriginal,
                             FechaFinEfectiva = @fechaFinEfectiva,
                             MontoMensual = @montoMensual,
                             Estado = @estado,
                             IdInquilino = @idInquilino,
                             IdInmueble = @idInmueble
                             WHERE IdContrato = @idContrato";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@fechaInicio", c.FechaInicio);
                command.Parameters.AddWithValue("@fechaFinOriginal", c.FechaFinOriginal);
                command.Parameters.AddWithValue("@fechaFinEfectiva", c.FechaFinEfectiva);
                command.Parameters.AddWithValue("@montoMensual", c.MontoMensual);
                command.Parameters.AddWithValue("@estado", c.Estado);
                command.Parameters.AddWithValue("@idInquilino", c.IdInquilino);
                command.Parameters.AddWithValue("@idInmueble", c.IdInmueble);
                command.Parameters.AddWithValue("@idContrato", c.IdContrato);

                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }


    public int BajaLogica(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE contrato
                             SET estado = 0
                             WHERE idContrato = @idContrato";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@idContrato", id);
                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }


    public int BajaReal(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"Delete contrato 
                           WHERE idContrato = @idContrato";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@idContrato", id);
                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }

    
    

    public IList<Contrato> ObtenerContratosPorInquilino(int idInquilino)
{
    IList<Contrato> lista = new List<Contrato>();
    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        string sql = @"
                SELECT
                c.IdContrato, c.FechaInicio, c.FechaFinOriginal, c.MontoMensual,
                inm.Direccion, inm.IdInmueble
            FROM contrato c
            JOIN inmueble inm ON c.IdInmueble = inm.IdInmueble
            WHERE c.IdInquilino = @idInquilino";

        using (MySqlCommand command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@idInquilino", idInquilino);
            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Contrato
                {
                    IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                    FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                    FechaFinOriginal = reader.IsDBNull(nameof(Contrato.FechaFinOriginal)) ? (DateTime?)null : reader.GetDateTime(nameof(Contrato.FechaFinOriginal)),
                    MontoMensual = reader.GetDecimal(nameof(Contrato.MontoMensual)),
                    // Carga el inmueble
                    InmuebleC = new Inmueble
                    {
                        IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion))
                    }
                });
            }
        }
    }
    return lista;
}
}