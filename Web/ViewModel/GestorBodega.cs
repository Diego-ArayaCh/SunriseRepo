using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Util;

namespace Web.ViewModel
{
    public class GestorBodega
    {
        public ViewModelMovimiento movimientoDetalle { get; private set; }
        
        //Implementación Singleton

        // Las propiedades de solo lectura solo se pueden establecer en la inicialización o en un constructor
        public static readonly GestorBodega Instancia;

        // Se llama al constructor estático tan pronto como la clase se carga en la memoria
        static GestorBodega()
        {
            // Si el carrito no está en la sesión, cree uno y guarde los items.
            if (HttpContext.Current.Session["GestorBodega"] == null)
            {
                Instancia = new GestorBodega();
                Instancia.movimientoDetalle = new ViewModelMovimiento();

                HttpContext.Current.Session["GestorBodega"] = Instancia;
            }
            else
            {
                // De lo contrario, obténgalo de la sesión.
                Instancia = (GestorBodega)HttpContext.Current.Session["GestorBodega"];
            }
        }

        // Un constructor protegido asegura que un objeto no se puede crear desde el exterior
        protected GestorBodega() { }

        public void AgregarActualizar(PRODUCTOS producto, int idProveedor,int cant)
        {
            if (movimientoDetalle.prodListDetalle.Exists(x => x.ID == producto.ID)) { 
                movimientoDetalle.historicoDetalle.Find(x => x.IDProducto == producto.ID).cantidad=cant;
            }
            else
            {
                HistDetalleEntradaSalida histDetalle = new HistDetalleEntradaSalida();
                histDetalle.IDProducto = producto.ID;
                histDetalle.cantidad = cant;
                histDetalle.IDProveedor = idProveedor;

                movimientoDetalle.historicoDetalle.Add(histDetalle);
            }
        }
        public String EliminarItem(int idProducto)
        {
            String mensaje = "";
            if (movimientoDetalle.historicoDetalle.Exists(x => x.IDProducto == idProducto))
            {
                var itemEliminar = movimientoDetalle.historicoDetalle.Single(x => x.IDProducto == idProducto);
                movimientoDetalle.historicoDetalle.Remove(itemEliminar);
                mensaje = SweetAlertHelper.Mensaje("Movimiento Producto", "Producto eliminado", SweetAlertMessageType.success);
            }
            return mensaje;

        }
    }
}