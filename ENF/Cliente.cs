//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ENF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cliente
    {
        public int IdCliente { get; set; }
        public Nullable<int> No_Cliente { get; set; }
        public Nullable<int> IdCadena { get; set; }
        public string Sucursal { get; set; }
        public Nullable<System.DateTime> InicioContrato { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<System.DateTime> FechaActualizacion { get; set; }
        public byte[] Imagen { get; set; }
    
        public virtual Cadena Cadena { get; set; }
    }
}
