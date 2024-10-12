using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class Conexion
    {
        public static string ConexionBD()
        {
            return "Data Source =.; Initial Catalog = JEnriquezCRUDClientes; User ID = sa; Password = pass@word1";
        }
    }
}
