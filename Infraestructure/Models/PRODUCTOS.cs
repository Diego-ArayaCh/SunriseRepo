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
    
    public partial class PRODUCTOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRODUCTOS()
        {
            this.HistDetalleCRUD = new HashSet<HistDetalleCRUD>();
            this.HistDetalleEntradaSalida = new HashSet<HistDetalleEntradaSalida>();
            this.ProdSuc = new HashSet<ProdSuc>();
            this.PROVEEDORES = new HashSet<PROVEEDORES>();
        }
    
        public int ID { get; set; }
        public Nullable<int> IDCategoria { get; set; }
        public Nullable<int> estado { get; set; }
        public string serial { get; set; }
        public Nullable<decimal> precio { get; set; }
        public string nombre { get; set; }
        public string marca { get; set; }
        public string detalle { get; set; }
        public string imagen { get; set; }
        public Nullable<int> stock { get; set; }
        public Nullable<int> cantMin { get; set; }
    
        public virtual CATEGORIA CATEGORIA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistDetalleCRUD> HistDetalleCRUD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistDetalleEntradaSalida> HistDetalleEntradaSalida { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProdSuc> ProdSuc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROVEEDORES> PROVEEDORES { get; set; }
    }
}
