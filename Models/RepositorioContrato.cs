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
                             (FechaInicio, FechaFinOriginal, FechaFinEfectiva, MontoMensual, Estado, IdInquilino, IdInmueble, usuarioCreador)
                             VALUES (@fechaInicio, @fechaFinOriginal, @fechaFinEfectiva, @montoMensual, @estado, @idInquilino, @idInmueble, @usuarioCreador);
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
                command.Parameters.AddWithValue("@usuarioCreador", c.IdUsuarioCredor);

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
                            i.nombre, i.apellido, inm.direccion, i.dni
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
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Dni = reader.GetString(nameof(Inquilino.Dni))
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


    public int BajaLogica(int id, int idUsuario)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE contrato
                             SET estado = 0, usuarioFinalizador = @usuarioFinalizador
                             WHERE idContrato = @idContrato";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@idContrato", id);
                command.Parameters.AddWithValue("@usuarioFinalizador", idUsuario);
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


    public IList<Contrato> ObtenerContratosPorInmueble(int idInmueble)
    {
        IList<Contrato> lista = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT  c.IdContrato, c.FechaInicio, c.FechaFinOriginal, c.FechaFinEfectiva, c.MontoMensual,
             c.Estado, c.IdInquilino, c.IdInmueble,
                i.nombre, i.apellido, inm.direccion
            FROM contrato c 
            JOIN inquilino i ON i.idInquilino = c.idInquilino
            JOIN inmueble inm ON c.idInmueble = inm.idInmueble
            WHERE c.IdInmueble = @idInmueble";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@idInmueble", idInmueble);
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

    public (IList<Contrato> Lista, int totalRegistro) verTodosPaginado(
    int paginaNro = 1, int paginaTam = 5,
    int? idInmueble = null, bool? estado = null) 
    {
        IList<Contrato> listaC = new List<Contrato>();
        int totalRegistro = 0;
        var filtros = new List<string>();

        

        
        if (idInmueble.HasValue)
        {
            filtros.Add($"c.IdInmueble = {idInmueble.Value}");
        }

       
        if (estado.HasValue)
        {
            int estadoDB = estado.Value ? 1 : 0;
            filtros.Add($"c.Estado = {estadoDB}");
        }

      
        string whereClausula = "";
        if (filtros.Count > 0)
        {
            whereClausula = " WHERE " + string.Join(" AND ", filtros);
        }

       
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            
            string sqlCount = $@"
            SELECT COUNT(*) 
            FROM contrato c 
            JOIN inquilino q ON c.IdInquilino = q.IdInquilino
            JOIN inmueble i ON c.IdInmueble = i.IdInmueble
            {whereClausula}";

            using (var countCommand = new MySqlCommand(sqlCount, connection))
            {
                totalRegistro = Convert.ToInt32(countCommand.ExecuteScalar());
            }

            
            int offset = (paginaNro - 1) * paginaTam;

            string sqlSelect = $@"
            SELECT 
                c.*, 
                q.Nombre AS InquilinoNombre, q.Apellido AS InquilinoApellido,
                i.Direccion, i.Tipo -- Campos de Inmueble (i)

            FROM contrato c
            JOIN inquilino q ON c.IdInquilino = q.IdInquilino
            JOIN inmueble i ON c.IdInmueble = i.IdInmueble
            
            {whereClausula}
            ORDER BY c.FechaInicio DESC
            LIMIT {paginaTam} OFFSET {offset}
        ";

            using (MySqlCommand command = new MySqlCommand(sqlSelect, connection))
            {
                var reader = command.ExecuteReader();

                 while (reader.Read())
                {
                    listaC.Add(new Contrato
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
                            Nombre = reader.GetString("InquilinoNombre"),
                            Apellido = reader.GetString("InquilinoApellido"),
                        },
                        InmuebleC = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion))
                        }
                    });
                }
            }
            return (listaC, totalRegistro);
        }
    }


}