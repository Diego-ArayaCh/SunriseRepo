//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infraestructure.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PROVEEDORES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROVEEDORES()
        {
            this.CONTACTO = new HashSet<CONTACTO>();
            this.HistDetalleCRUD = new HashSet<HistDetalleCRUD>();
            this.HistDetalleEntradaSalida = new HashSet<HistDetalleEntradaSalida>();
            this.PRODUCTOS = new HashSet<PRODUCTOS>();
        }
    
        public int ID { get; set; }
        public Nullable<int> estado { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public Nullable<int> IDPais { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CONTACTO> CONTACTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistDetalleCRUD> HistDetalleCRUD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistDetalleEntradaSalida> HistDetalleEntradaSalida { get; set; }
        public virtual PAIS PAIS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUCTOS> PRODUCTOS { get; set; }
    }
}
