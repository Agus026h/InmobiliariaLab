using System;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration; 

namespace devs.Models;

public class RepositorioPago : Conexion
{

    public RepositorioPago(IConfiguration configuration) : base(configuration)
    {
    }

    public int Alta(Pago p)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            
            string sql = @"INSERT INTO pago 
                             (NumPago, FechaPago, Importe, Concepto, Estado, IdContrato, UsuarioCreador)
                             VALUES (@numPago, @fechaPago, @importe, @concepto, @estado, @idContrato, @usuarioCreador);
                             SELECT LAST_INSERT_ID();";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@numPago", p.NumPago);
                command.Parameters.AddWithValue("@fechaPago", p.FechaPago);
                command.Parameters.AddWithValue("@importe", p.Importe);
                command.Parameters.AddWithValue("@concepto", p.Concepto);
                command.Parameters.AddWithValue("@estado", (int)p.Estado);
                command.Parameters.AddWithValue("@idContrato", p.IdContrato);
                command.Parameters.AddWithValue("@usuarioCreador", p.UsuarioCreador);

                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                p.IdPago = res;
            }
        }
        return res;
    }
    //metodo de listado con filtro
    public (IList<Pago> Lista, int totalRegistro) verTodosPaginado(
    int paginaNro = 1, int paginaTam = 5,
    int? contratoId = null, string estado = null, string direccionInmueble = null)
    {
        IList<Pago> listaP = new List<Pago>();
        int totalRegistro = 0;
        string whereClausula = "";

        

       
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

       

        if (contratoId.HasValue)
        {
            agregarFiltro($"p.IdContrato = {contratoId.Value}");
        }

        if (!string.IsNullOrEmpty(estado) && Enum.TryParse(typeof(EstadoPago), estado, true, out object parsedEstado) )
        {
            
            agregarFiltro($"p.Estado = '{(int)parsedEstado}'");
        }

        if (!string.IsNullOrEmpty(direccionInmueble))
        {
           
            agregarFiltro($"i.Direccion LIKE '%{direccionInmueble}%'");
        }

        string filtro = whereClausula;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            
            string contador = @$"
            SELECT COUNT(*) 
            FROM pago p 
            JOIN contrato c ON p.IdContrato = c.IdContrato
            JOIN inmueble i ON c.IdInmueble = i.IdInmueble 
            {filtro}";

            using (var countCommand = new MySqlCommand(contador, connection))
            {
                countCommand.CommandType = CommandType.Text;
                totalRegistro = Convert.ToInt32(countCommand.ExecuteScalar());
            }

            
            String sql = @$"
            SELECT 
                p.IdPago, p.NumPago, p.FechaPago, p.Importe, p.Concepto, p.Estado, p.IdContrato, p.UsuarioCreador, p.UsuarioAnulador,
                c.FechaInicio, c.MontoMensual, c.IdInmueble, c.IdInquilino,
                i.Direccion, i.Tipo, i.Uso, i.Ambientes, i.Precio, 
                ui.Nombre AS InquilinoNombre, ui.Apellido AS InquilinoApellido, 
                uc.Nombre AS CreadorNombre, uc.Apellido AS CreadorApellido, 
                ua.Nombre AS AnuladorNombre, ua.Apellido AS AnuladorApellido 
            FROM pago p
            JOIN contrato c ON p.IdContrato = c.IdContrato
            JOIN inmueble i ON c.IdInmueble = i.IdInmueble 
            JOIN inquilino ui ON c.IdInquilino = ui.IdInquilino 
            JOIN usuario uc ON p.UsuarioCreador = uc.IdUsuario
            LEFT JOIN usuario ua ON p.UsuarioAnulador = ua.IdUsuario

            {filtro} 
            ORDER BY p.FechaPago DESC
            LIMIT {paginaTam} OFFSET {(paginaNro - 1) * paginaTam} 
        ";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    
                    Pago p = CrearPago(reader, true);
                    listaP.Add(p);
                }
                
                 connection.Close(); 
            }
            return (listaP, totalRegistro);
        }
    }


    //metodo para crear pagos
    private Pago CrearPago(MySqlDataReader reader, bool conRelaciones = false)
    {
        var p = new Pago
        {
            IdPago = reader.GetInt32(nameof(Pago.IdPago)),
            NumPago = reader.GetInt32(nameof(Pago.NumPago)),
            FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
            Importe = reader.GetDecimal(nameof(Pago.Importe)),
            Concepto = reader.GetString(nameof(Pago.Concepto)),
            Estado = (EstadoPago)reader.GetInt32(nameof(Pago.Estado)),
            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
            UsuarioCreador = reader.GetInt32(nameof(Pago.UsuarioCreador)),
            UsuarioAnulador = reader.IsDBNull(nameof(Pago.UsuarioAnulador)) ? (int?)null : reader.GetInt32(nameof(Pago.UsuarioAnulador)),
        };

        if (conRelaciones)
        {

            p.ContratoC = new Contrato
            {
                IdContrato = p.IdContrato,
                FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                MontoMensual = reader.GetDecimal(nameof(Contrato.MontoMensual)),
                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),


                InmuebleC = new Inmueble
                {
                    IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                    Direccion = reader.GetString(nameof(Inmueble.Direccion)) 
                },


                InquilinoC = new Inquilino
                {
                    IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                    Nombre = reader.GetString("InquilinoNombre"),
                    Apellido = reader.GetString("InquilinoApellido")
                }
            };


            p.UsuarioCreadorC = new Usuario
            {

                IdUsuario = p.UsuarioCreador,
                Nombre = reader.GetString("CreadorNombre"),
                Apellido = reader.GetString("CreadorApellido")
            };


            if (!reader.IsDBNull(nameof(Pago.UsuarioAnulador)))
            {
                p.UsuarioAnuladorC = new Usuario
                {
                    IdUsuario = p.UsuarioAnulador.Value,
                    Nombre = reader.GetString("AnuladorNombre"),
                    Apellido = reader.GetString("AnuladorApellido")
                };
            }
        }

        return p;
    }


    public IList<Pago> VerTodos()
    {
        IList<Pago> lista = new List<Pago>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"
            SELECT 
                p.IdPago, p.NumPago, p.FechaPago, p.Importe, p.Concepto, p.Estado, p.IdContrato, p.UsuarioCreador, p.UsuarioAnulador,
                
                c.FechaInicio, c.MontoMensual, c.IdInmueble, c.IdInquilino,
                
                inm.Direccion,
               
                i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
    
                uc.Nombre AS CreadorNombre, uc.Apellido AS CreadorApellido, 
                ua.Nombre AS AnuladorNombre, ua.Apellido AS AnuladorApellido
            FROM pago p
            JOIN contrato c ON p.IdContrato = c.IdContrato
            JOIN inmueble inm ON c.IdInmueble = inm.IdInmueble  
            JOIN inquilino i ON c.IdInquilino = i.IdInquilino  
            JOIN usuario uc ON p.UsuarioCreador = uc.IdUsuario
            LEFT JOIN usuario ua ON p.UsuarioAnulador = ua.IdUsuario
            ORDER BY p.FechaPago DESC"; // ordenanos por fecha

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(CrearPago(reader, true));
                }
            }
        }
        return lista;
    }


    public Pago? BuscarId(int id)
    {
        Pago? pago = null;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"
            SELECT 
                p.IdPago, p.NumPago, p.FechaPago, p.Importe, p.Concepto, p.Estado, p.IdContrato, p.UsuarioCreador, p.UsuarioAnulador,
                
                c.FechaInicio, c.MontoMensual, c.IdInmueble, c.IdInquilino,
                
                inm.Direccion,
               
                i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
    
                uc.Nombre AS CreadorNombre, uc.Apellido AS CreadorApellido, 
                ua.Nombre AS AnuladorNombre, ua.Apellido AS AnuladorApellido
            FROM pago p
            JOIN contrato c ON p.IdContrato = c.IdContrato
            JOIN inmueble inm ON c.IdInmueble = inm.IdInmueble  
            JOIN inquilino i ON c.IdInquilino = i.IdInquilino  
            JOIN usuario uc ON p.UsuarioCreador = uc.IdUsuario
            LEFT JOIN usuario ua ON p.UsuarioAnulador = ua.IdUsuario
            WHERE idPago = @id";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    pago = CrearPago(reader, true);
                }
            }
        }
        return pago;
    }



    public IList<Pago> ObtenerPagosPorContrato(int idContrato)
    {
        IList<Pago> lista = new List<Pago>();
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT * FROM pago WHERE IdContrato = @idContrato ORDER BY NumPago ASC";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@idContrato", idContrato);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(CrearPago(reader));
                }
            }
        }
        return lista;
    }


    public int Modificar(Pago p)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            // segun la narrativa solamente se puede editar el concepto
            string sql = @"UPDATE pago SET
                                 Concepto = @concepto
                             WHERE IdPago = @idPago";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@concepto", p.Concepto);
                command.Parameters.AddWithValue("@idPago", p.IdPago);

                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }


    public int BajaLogica(int idPago, int idUsuarioAnulador)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {

            string sql = @"UPDATE pago
                             SET Estado = 2,
                                 UsuarioAnulador = @idUsuarioAnulador
                             WHERE IdPago = @idPago";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@idPago", idPago);
                command.Parameters.AddWithValue("@idUsuarioAnulador", idUsuarioAnulador);

                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }
    public int MarcarComoPagado(int idPago)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            
            string sql = @"UPDATE pago SET
                                Estado = @estadoPagado 
                            WHERE IdPago = @idPago 
                            AND Estado = @estadoPendiente"; // Solo si esta Pendiente

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                
               
                command.Parameters.AddWithValue("@estadoPagado", EstadoPago.Pagado); 
                command.Parameters.AddWithValue("@estadoPendiente", EstadoPago.Pendiente);
                command.Parameters.AddWithValue("@idPago", idPago);

                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }
        return res;
    }

}