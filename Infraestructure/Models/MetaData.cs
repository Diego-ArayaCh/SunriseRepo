using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Models
{
    internal partial class CategoriaMetaData
    {
        [Display(Name = "Categoría")]
        public int ID { get; set; }
        [Display(Name = "Categoría")]
        public string descripcion { get; set; }
        [Display(Name = "Estado")]
        public Nullable<int> estado { get; set; }
    }
    internal partial class ContactoMetaData
    {
        [Display(Name = "Identificación")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public int ID { get; set; }
        [Display(Name = "Identificación Proveedor")]
        public Nullable<int> IDProv { get; set; }
        [Display(Name = "Estado")]
        public Nullable<int> estado { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string nombre { get; set; }
        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string correo { get; set; }
        [Display(Name = "Teléfono ")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string telefono { get; set; }
    }
    internal partial class HistoricoDetalleMetaData
    {
        [Display(Name = "Identificación Histórico")]
        public int IDHistorico { get; set; }
        [Display(Name = "Identificación Producto")]
        public int IDProducto { get; set; }
        [Display(Name = "Identificación Proveedor")]
        public Nullable<int> IDProveedor { get; set; }
        [Display(Name = "Identificación Usuario")]
        public Nullable<int> IDUsuario { get; set; }
        [Display(Name = "Identificación Contacto")]
        public Nullable<int> IDContacto { get; set; }
        [Display(Name = "Tipo Movimiento")]
        public int TipoCRUD { get; set; }
    }
    internal partial class HistoricoDetalleEntradaSalidaMetaData
    {
        [Display(Name = "Identificación Histórico")]
        public int IDHistorico { get; set; }
        [Display(Name = "Identificación Producto")]
        public int IDProducto { get; set; }
        [Display(Name = "Sucursal")]
        public Nullable<int> IDSucursalEntra { get; set; }
        [Display(Name = "Sucursal")]
        public Nullable<int> IDSucursalSale { get; set; }
        [Display(Name = "Identificación Proveedor")]
        public Nullable<int> IDProveedor { get; set; }
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public Nullable<int> cantidad { get; set; }
    }
    internal partial class HistoricoMetaData
    {
        [Display(Name = "Identificación")]
        public int ID { get; set; }
        [Display(Name = "Fecha y Hora")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string fechaHora { get; set; }
        [Display(Name = "Identificación Usuario")]
        public Nullable<int> IDUsuario { get; set; }
        [Display(Name = "Tipo de Movimiento")]
        public Nullable<int> tipoMov { get; set; }
        [Display(Name = "Detalle")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string detalle { get; set; }
    }
    internal partial class MovimientoMetaData
    {
        [Display(Name = "Identificación")]
        public int ID { get; set; }
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }
        [Display(Name = "Estado")]
        public Nullable<int> estado { get; set; }
    }
    internal partial class PaisMetaData
    {
        [Display(Name = "Identificación")]
        public int ID { get; set; }
        [Display(Name = "País")]
        public string nombre { get; set; }
    }
    internal partial class ProductoSucursalMetaData
    {
        [Display(Name = "Identificación Producto")]
        public int IDProducto { get; set; }
        [Display(Name = "Identificación Sucursal")]
        public int IDSucursal { get; set; }
        [Display(Name = "Estado")]
        public Nullable<int> estado { get; set; }
        [Display(Name = "Cantidad")]
        public Nullable<int> cant { get; set; }
    }
    internal partial class ProductoMetaData
    {
        [Display(Name = "Identificación")]
        public int ID { get; set; }
        [Display(Name = "Identificación Categoría")]
        public Nullable<int> IDCategoria { get; set; }
        [Display(Name = "Estado")]
        public Nullable<int> estado { get; set; }
        [Display(Name = "Serial")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string serial { get; set; }
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public Nullable<decimal> precio { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string nombre { get; set; }
        [Display(Name = "Marca")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string marca { get; set; }
        [Display(Name = "Detalle")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string detalle { get; set; }
        [Display(Name = "Imagen")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public byte[] imagen { get; set; }
        [Display(Name = "Stock")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public Nullable<int> stock { get; set; }
        [Display(Name = "Cantidad Mínima")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public Nullable<int> cantMin { get; set; }
    }
    internal partial class ProveedorMetaData
    {
        [Display(Name = "Identificación")]
        public int ID { get; set; }
        [Display(Name = "Estado")]
        public Nullable<int> estado { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string nombre { get; set; }
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string direccion { get; set; }
        [Display(Name = "Identificación País")]
        public Nullable<int> IDPais { get; set; }
    }
    internal partial class RolMetaData
    {
        [Display(Name = "Identificación")]
        public int ID { get; set; }
        [Display(Name = "Descripción ")]
        public string descripcion { get; set; }
    }
    internal partial class SucursalMetaData
    {
        [Display(Name = "Identificación")]
        public int ID { get; set; }
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Display(Name = "Dirección")]
        public string direccion { get; set; }
        [Display(Name = "Estado")]
        public Nullable<int> estado { get; set; }
    }
    internal partial class TipoCRUDMetaData
    {
        [Display(Name = "Identificación")]
        public int ID { get; set; }
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }
    }
    internal partial class UsuarioMetaData
    {
        [Display(Name = "Identificación")]
        public int ID { get; set; }
        [Display(Name = "Número de Cédula")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string cedula { get; set; }
        [Display(Name = "Estado")]
        public Nullable<int> estado { get; set; }
        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "{0} no tiene formato válido")]
        public string correo { get; set; }
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string contrasenha { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string nombre { get; set; }
        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string apellidos { get; set; }
        [Display(Name = "Identificación de Rol")]
        public Nullable<int> IDRol { get; set; }
        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string telefono { get; set; }
    }

}
