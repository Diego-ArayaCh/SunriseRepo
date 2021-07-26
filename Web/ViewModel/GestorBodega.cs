using ApplicationCore.Services;
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

        public bool AgregarActualizar(PRODUCTOS producto, int? idProveedor,int cant)
        {
            if (movimientoDetalle.historicoDetalle.Exists(x => x.IDProveedor !=idProveedor && x.IDProducto == producto.ID))
            {
                return true;
            }
            if (movimientoDetalle.historicoDetalle.Exists(x => x.IDProducto == producto.ID)) { 
                movimientoDetalle.historicoDetalle.Find(x => x.IDProducto == producto.ID).cantidad=cant;
                return false;
            }
            else
            {
                HistDetalleEntradaSalida histDetalle = new HistDetalleEntradaSalida();
                histDetalle.IDProducto = producto.ID;
                histDetalle.cantidad = cant;
                histDetalle.IDProveedor = idProveedor.Value;
                histDetalle.PROVEEDORES = new ServiceProveedores().GetProveedorByID(idProveedor.Value);
                histDetalle.PRODUCTOS = producto;

                movimientoDetalle.historicoDetalle.Add(histDetalle);
                return false;
            }
        }
        public bool AgregarActualizarSalida(PRODUCTOS producto, int? idSucursal, int cant)
        {
            if (movimientoDetalle.historicoDetalle.Exists(x => x.IDSucursalSale != idSucursal && x.IDProducto == producto.ID))
            {
                return true;
            }
            if (movimientoDetalle.historicoDetalle.Exists(x => x.IDProducto == producto.ID))
            {
                movimientoDetalle.historicoDetalle.Find(x => x.IDProducto == producto.ID).cantidad = cant;
                return false;
            }
            else
            {
                HistDetalleEntradaSalida histDetalle = new HistDetalleEntradaSalida();
                histDetalle.IDProducto = producto.ID;
                histDetalle.cantidad = cant;
                histDetalle.IDSucursalSale = idSucursal.Value;
             
                histDetalle.PRODUCTOS = producto;

                movimientoDetalle.historicoDetalle.Add(histDetalle);
                return false;
            }
        }
        public String EliminarProducto(int idProducto)
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
        public int  GetHistDetalle(int idProd)
        {

            int tipoMov = 0;
            foreach (var item in Instancia.movimientoDetalle.historicoDetalle)
            {
                if (item.IDProducto == idProd)
                {
                    if (item.IDSucursalSale != null)
                    {
                        tipoMov = 2;
                    }
                    else
                    {
                        tipoMov = 1;
                    }
                }
            }



            return tipoMov;
        }
        public void VaciarMovimiento()
        {
            movimientoDetalle.historicoDetalle = new List<HistDetalleEntradaSalida>();
        }
    }
}