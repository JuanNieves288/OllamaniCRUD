using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Clientes
    {

        public static ML.Result ChangeStatus(bool Activo, int IdCandidato)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (ENF.JEnriquezCRUDClientesEntities context = new ENF.JEnriquezCRUDClientesEntities())
                {
                    context.ClienteChangeStatus(Activo, IdCandidato);

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return result;
        }

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using(SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = DL.Conexion.ConexionBD();
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("ClienteGetAll", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dataTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);  
                    adapter.Fill(dataTable);    

                    if(dataTable.Rows.Count > 0 )
                    {
                        result.Objects = new List<object>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            ML.Cliente cliente = new ML.Cliente
                            {
                                IdCliente = Convert.ToInt32(row["IdCliente"]),
                                Cadena = new ML.Cadena
                                {
                                    IdCadena = Convert.ToInt32(row["IdCadena"]),
                                    Nombre = row["Nombre"].ToString()
                                },
                                Sucursal = row["Sucursal"].ToString(),
                                InicioContrato = Convert.ToDateTime(row["InicioContrato"]).ToString("dd/MM/yyyy"),
                                Activo = Convert.ToBoolean(row["Activo"]),
                                FechaActualizacion = Convert.ToDateTime(row["FechaActualizacion"]).ToString("dd/MM/yyyy"),
                                Imagen = row["Imagen"] as byte[],
                                No_Cliente = Convert.ToInt32(row["No_Cliente"])
                            };

                            result.Objects.Add(cliente);
                        }
                        result.Success = true;
                    }else { result.Success = false; }
                }
            }
            catch(Exception ex)
            {
                result.Exception = ex;
                result.Success = false;
            }
            return result;
        }


        public static ML.Result Add(ML.Cliente cliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(DL.Conexion.ConexionBD()))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("ClienteAdd", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdCadena", cliente.Cadena.IdCadena);
                        cmd.Parameters.AddWithValue("@Sucursal", cliente.Sucursal);
                        DateTime fecha = DateTime.ParseExact(cliente.InicioContrato, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        cmd.Parameters.AddWithValue("@InicioContrato", fecha.Date);
                        cmd.Parameters.AddWithValue("@Activo", cliente.Activo);
                        cmd.Parameters.AddWithValue("@FechaActualizacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Imagen", (object)cliente.Imagen ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@No_Cliente", cliente.No_Cliente);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        result.Success = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.Success = false;
            }
            return result;
        }

        public static ML.Result Update(ML.Cliente cliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(DL.Conexion.ConexionBD()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("ClienteUpdate", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                        cmd.Parameters.AddWithValue("@IdCadena", cliente.Cadena.IdCadena);
                        cmd.Parameters.AddWithValue("@Sucursal", cliente.Sucursal);
                        if (string.IsNullOrEmpty(cliente.InicioContrato))
                        {
                            cmd.Parameters.AddWithValue("@InicioContrato", DBNull.Value); 
                        }
                        else
                        {
                            DateTime fecha = DateTime.ParseExact(cliente.InicioContrato, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            cmd.Parameters.AddWithValue("@InicioContrato", fecha.Date);
                        }

                        cmd.Parameters.AddWithValue("@Activo", cliente.Activo);
                        cmd.Parameters.AddWithValue("@FechaActualizacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Imagen", (object)cliente.Imagen ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@No_Cliente", cliente.No_Cliente);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        result.Success = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.Success = false;
            }
            return result;
        }




        public static ML.Result Delete(int IdCliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection(DL.Conexion.ConexionBD()))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("ClienteDelete", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdCliente", IdCliente); 

                        int rowsAffected = cmd.ExecuteNonQuery(); 

                        result.Success = rowsAffected > 0; 
                    }
                }
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.Success = false;
            }
            return result;
        }   

        public static ML.Result GetById(int IdCliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = DL.Conexion.ConexionBD();
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("ClienteGetById", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);

                    DataTable dataTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        ML.Cliente cliente = new ML.Cliente();
                        DataRow row = dataTable.Rows[0]; 

                        cliente.IdCliente = IdCliente; 
                        cliente.Cadena = new ML.Cadena();
                        cliente.Cadena.IdCadena = Convert.ToInt32(row["IdCadena"]);
                        cliente.Sucursal = row["Sucursal"].ToString();
                        cliente.InicioContrato = ((DateTime)row["InicioContrato"]).ToString("dd/MM/yyyy");
                        cliente.Activo = Convert.ToBoolean(row["Activo"]);
                        cliente.FechaActualizacion = ((DateTime)row["FechaActualizacion"]).ToString("dd/MM/yyyy");
                        cliente.Imagen = row["Imagen"] as byte[];
                        cliente.No_Cliente = Convert.ToInt32(row["No_Cliente"]);
                        result.Object = cliente; 
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false; 
                    }
                }
            }
            catch (Exception ex)
            {
                result.Exception = ex; 
                result.Success = false;
            }
            return result;
        }

     




        public static ML.Result GetAllEF()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (ENF.JEnriquezCRUDClientesEntities context = new ENF.JEnriquezCRUDClientesEntities())
                {
                    var clientes = context.ClienteGetAll().ToList();
                    if (clientes != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in clientes)
                        {
                            ML.Cliente data = new ML.Cliente
                            {
                                IdCliente = item.IdCliente,
                                No_Cliente = item.No_Cliente.Value,
                                Sucursal = item.Sucursal,
                                InicioContrato = item.InicioContrato.Value.ToString("dd/MM/yyyy"),
                                Activo = Convert.ToBoolean(item.Activo),
                                FechaActualizacion = item.FechaActualizacion.Value.ToString("dd/MM/yyyy"),
                                Cadena = new ML.Cadena
                                {
                                    IdCadena = Convert.ToInt32(item.IdCadena),
                                    Nombre = item.Nombre
                                },
                                 Imagen = item.Imagen
                            };

                            result.Objects.Add(data);
                        }

                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMessage = "No existen registros en la tabla";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result AddEFs(ML.Cliente cliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (ENF.JEnriquezCRUDClientesEntities context = new ENF.JEnriquezCRUDClientesEntities())
                {
                    int filasAfectadas = context.ClienteAdd
                        (
                            cliente.Cadena.IdCadena, 
                            cliente.Sucursal,
                            DateTime.ParseExact(cliente.InicioContrato, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            cliente.Activo,
                            DateTime.Now,
                            cliente.Imagen,
                            cliente.No_Cliente
                        );

                    result.Success = filasAfectadas > 0;
                    if (!result.Success)
                    {
                        result.ErrorMessage = "No se pudo agregar.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "Error al intentar agregar: " + ex.Message;
            }
            return result;
        }




        public static ML.Result UpdateEF(ML.Cliente cliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (ENF.JEnriquezCRUDClientesEntities context = new ENF.JEnriquezCRUDClientesEntities())
                {
                    var clienteExistente = context.Clientes.Find(cliente.IdCliente);
                    if (clienteExistente != null)
                    {
                        clienteExistente.No_Cliente = cliente.No_Cliente;
                        clienteExistente.Sucursal = cliente.Sucursal;
                        clienteExistente.InicioContrato = DateTime.ParseExact(cliente.InicioContrato, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        clienteExistente.Activo = cliente.Activo;
                        clienteExistente.FechaActualizacion = DateTime.Now;
                        clienteExistente.IdCadena = cliente.Cadena.IdCadena;
                        clienteExistente.Imagen = cliente.Imagen;

                        int filasAfectadas = context.SaveChanges();

                        if (filasAfectadas > 0)
                        {
                            result.Success = true;
                        }
                        else
                        {
                            result.Success = false;
                            result.ErrorMessage = "No se pudo actualizar.";
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMessage = "Cliente no encontrado.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "Error al intentar actualizar: " + ex.Message;
            }
            return result;
        }
        public static ML.Result DeleteEF(int idCliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (var context = new ENF.JEnriquezCRUDClientesEntities())
                {
                    var parameter = new SqlParameter("@IdCliente", idCliente);
                    context.Database.ExecuteSqlCommand("EXEC ClienteDelete @IdCliente", parameter);
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "Error al intentar eliminar el cliente: " + ex.Message;
            }
            return result;
        }

    

        public static ML.Result GetByIdEF(int IdCliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (ENF.JEnriquezCRUDClientesEntities context = new ENF.JEnriquezCRUDClientesEntities())
                {
                    var client = (from BD in context.Clientes
                                   where BD.IdCliente == IdCliente
                                   select BD).SingleOrDefault();

                    if (client != null)
                    {
                        ML.Cliente Item = new ML.Cliente();
                        Item.IdCliente = client.IdCliente;
                        Item.No_Cliente = client.No_Cliente.Value;
                        Item.InicioContrato = client.InicioContrato.Value.ToString("dd/MM/yyyy");
                        Item.FechaActualizacion = client.FechaActualizacion.Value.ToString("dd/MM/yyyy");
                        Item.Sucursal = client.Sucursal;
                        Item.Activo = Convert.ToBoolean(client.Activo);
                        Item.Cadena = new ML.Cadena();
                        Item.Cadena.IdCadena = client.Cadena.IdCadena;
                        Item.Cadena.Nombre = client.Cadena.Nombre;
                        Item.Imagen= client.Imagen;

                        result.Object = Item;
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }




        public static ML.Result GetAllLINQ()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (ENF.JEnriquezCRUDClientesEntities context = new ENF.JEnriquezCRUDClientesEntities())
                {
                    var clientes = (from c in context.Clientes 
                                    select c).ToList();

                    if (clientes.Any())
                    {
                        result.Objects = new List<object>();
                        foreach (var item in clientes)
                        {
                            ML.Cliente data = new ML.Cliente
                            {
                                IdCliente = item.IdCliente,
                                No_Cliente = item.No_Cliente.Value,
                                Sucursal = item.Sucursal,
                                InicioContrato = item.InicioContrato.Value.ToString("dd/MM/yyyy"),
                                Activo = Convert.ToBoolean(item.Activo),
                                FechaActualizacion = item.FechaActualizacion.Value.ToString("dd/MM/yyyy"),
                                Imagen = item.Imagen,
                                Cadena = new ML.Cadena
                                {
                                    IdCadena = Convert.ToInt32(item.IdCadena),
                                    Nombre = item.Cadena.Nombre,
                                }
                                
                            };

                            result.Objects.Add(data);
                        }

                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMessage = "No existen registros en la tabla";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
        public static ML.Result GetByILINQ(int IdCliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (ENF.JEnriquezCRUDClientesEntities context = new ENF.JEnriquezCRUDClientesEntities())
                {
                    var client = context.Clientes
                        .Where(c => c.IdCliente == IdCliente)
                        .Select(c => new
                        {
                            c.No_Cliente,
                            c.IdCadena,
                            c.Sucursal,
                            c.InicioContrato,
                            c.Activo,
                            c.FechaActualizacion,
                            c.Imagen
                        })
                        .SingleOrDefault();

                    if (client != null)
                    {
                        ML.Cliente Item = new ML.Cliente
                        {
                            IdCliente = IdCliente,
                            No_Cliente = client.No_Cliente.Value,
                            Sucursal = client.Sucursal,
                            InicioContrato = client.InicioContrato.Value.ToString("dd/MM/yyyy"),
                            FechaActualizacion = client.FechaActualizacion.Value.ToString("dd/MM/yyyy"),
                            Activo = Convert.ToBoolean(client.Activo),
                            Imagen = client.Imagen,
                            Cadena = new ML.Cadena
                            {
                                IdCadena = client.IdCadena.Value,
                            }
                        };

                        result.Object = Item;
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result DeleteLINQ(int idCliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (ENF.JEnriquezCRUDClientesEntities context = new ENF.JEnriquezCRUDClientesEntities())
                {
                    var cliente = context.Clientes.Find(idCliente);

                    if (cliente != null)
                    {
                        context.Clientes.Remove(cliente);
                        context.SaveChanges();

                        result.Success = true;

                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMessage = "Cliente no encontrado.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "Error al intentar eliminar el cliente: " + ex.Message;
            }
            return result;
        }

        public static ML.Result AddLINQ(ML.Cliente cliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (ENF.JEnriquezCRUDClientesEntities context = new ENF.JEnriquezCRUDClientesEntities())
                {
                    var existente = context.Clientes
                        .Where(c => c.No_Cliente == cliente.No_Cliente)
                        .FirstOrDefault();

                    ENF.Cliente nueva = new ENF.Cliente
                    {
                        No_Cliente = cliente.No_Cliente,
                        Sucursal = cliente.Sucursal,
                        InicioContrato = DateTime.ParseExact(cliente.InicioContrato, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Activo = cliente.Activo,
                        FechaActualizacion = DateTime.Now,
                        Imagen = cliente.Imagen,
                        IdCadena = cliente.Cadena.IdCadena
                    };

                    context.Clientes.Add(nueva);
                    int filasAfectadas = context.SaveChanges();

                    if (filasAfectadas>0)
                    {
                        result.Success = true;
                    } else
                    {
                        result.Success = false;
                        result.ErrorMessage = "Error al insertar";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "Error al intentar agregar: " + ex.Message;
            }

            return result;
        }

        public static ML.Result UpdateLINQ(ML.Cliente cliente)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (var context = new ENF.JEnriquezCRUDClientesEntities())
                {
                    var clienteExistente = context.Clientes
                        .FirstOrDefault(c => c.IdCliente == cliente.IdCliente);

                    if (clienteExistente != null)
                    {
                        clienteExistente.No_Cliente = cliente.No_Cliente;
                        clienteExistente.Sucursal = cliente.Sucursal;
                        clienteExistente.InicioContrato = DateTime.ParseExact(cliente.InicioContrato, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        clienteExistente.Activo = cliente.Activo;
                        clienteExistente.FechaActualizacion = DateTime.Now;
                        clienteExistente.IdCadena = cliente.Cadena.IdCadena;
                        clienteExistente.Imagen = cliente.Imagen;
                        context.SaveChanges();
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMessage = "Cliente no encontrado.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "Error al intentar actualizar: " + ex.Message;
            }
            return result;
        }

    }
}
