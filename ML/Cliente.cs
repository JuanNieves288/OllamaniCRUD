using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ML
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        [Display(Name = "Id del Cliente")]
        public int No_Cliente { get; set; }
        public string Nombre { get; set; }
        public string Sucursal { get; set; }
        [Display(Name = "Inicio del Contrato")]
        public string InicioContrato { get; set; }
		public bool Activo { get; set; }
        [Display(Name = "Fecha de Actualizacion")]
        public string FechaActualizacion { get; set; }
		public List<object> Clientes { get; set; }
        public byte[] Imagen { get; set; }
        public string ImagenBase64 { get; set; }
        public Cadena Cadena { get; set; }
        public Cliente()
        {
            Cadena = new Cadena(); 
        }
    }
}
