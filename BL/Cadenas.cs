using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Cadenas
    {
        public static ML.Result GetAllRol()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = DL.Conexion.ConexionBD();
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("CadenasGetAll", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dataTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                           ML.Cadena cadenas = new ML.Cadena();
                            cadenas.IdCadena = Convert.ToInt32(row["IdCadena"]);
                            cadenas.Nombre = row["Nombre"].ToString();
                            cadenas.Activo = Convert.ToBoolean(row["Activo"]);
                            result.Objects.Add(cadenas);
                        }
                        result.Success = true;
                    }
                    else { result.Success = false; }
                }
            }
            catch(Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}