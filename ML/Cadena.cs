using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ML
{
    public class Cadena
    {
        [Display(Name = "Cadenas")]
        public int IdCadena { get ; set; }
        public string Nombre { get; set;}
        public bool Activo { get; set; }
        public List<object> Cadenas { get; set; }
    }
}

