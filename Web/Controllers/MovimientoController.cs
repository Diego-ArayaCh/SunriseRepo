using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Util;
using Web.ViewModel;

namespace Web.Controllers
{
    public class MovimientoController : Controller
    {


        //[HttpPost]
        public ActionResult AgregarActualizarProducto(int idProveedor, int cantidad, int id)
        {
            IEnumerable<HistDetalleEntradaSalida> model=new List<HistDetalleEntradaSalida>();
            try
            {
                
                if (cantidad > 0)
                {
                    
                   bool validacion= GestorBodega.Instancia.AgregarActualizar(new ServiceProductos().GetProductoByID(id), idProveedor, cantidad);
                    if (validacion)
                    {
                        TempData["Notificacion"] = Util.SweetAlertHelper.Mensaje(
                                                                   "Error",
                                                                   "El producto está agregado con otro proveedor",
                                                                   SweetAlertMessageType.warning);
                        TempData.Keep();
                        ViewBag.Mensaje= Util.SweetAlertHelper.Mensaje(
                                                                   "Error",
                                                                   "El producto está agregado con otro proveedor",
                                                                   SweetAlertMessageType.warning);
                    }
                    model = GestorBodega.Instancia.movimientoDetalle.historicoDetalle;
                }
                else
                {

                }
                //ViewBag.ListaProveedores = listaProveedores_lst();
                //ViewBag.IDProveedor = listaProveedores();
                return PartialView("_MovimientoDetalle", model);
            }
            catch (Exception ex)
            {

                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData.Keep();
                return RedirectToAction("Default", "Error");
            }

        }
        public ActionResult EliminarProducto(int idProd)
        {
            IEnumerable<HistDetalleEntradaSalida> model = new List<HistDetalleEntradaSalida>();
            try
            {
                GestorBodega.Instancia.EliminarProducto(idProd);
                ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                   "Eliminado",
                                                                   "El producto se elimino del movimiento",
                                                                   SweetAlertMessageType.success);

                model = GestorBodega.Instancia.movimientoDetalle.historicoDetalle;
                
                return PartialView("_MovimientoDetalle", model);
            }
            catch (Exception ex)
            {

                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData.Keep();
                return RedirectToAction("Default", "Error");
            }

        }
        public PartialViewResult MovimientoDetalle()
        {
            return PartialView();
        }
        
        [HttpPost]
        public ActionResult Save()
        {
           
            return View();
        }
        
        public ActionResult SeleccionarProducto(int id)
        {
            PRODUCTOS model = new ServiceProductos().GetProductoByID(id);
            return PartialView("_ModalProductos", model);
        }


        //
        //
        //====================================================================
        //=================== COMBOS DE LA APLICACION ========================
        //====================================================================
        //
        private SelectList listaProveedores(int IDProveedor = 0)
        {
            //Lista de Categorias
            ServiceProductos _ServiceProducto = new ServiceProductos();
            IEnumerable<PROVEEDORES> listaProveedores = _ServiceProducto.GetProveedores();

            return new SelectList(listaProveedores, "ID", "nombre", IDProveedor);
        } //comboBox de proveedores
        
        private SelectList listaSucursales(int IDSucursal = 0)
        {
            //Lista de Categorias
            ServiceMovimiento _Service = new ServiceMovimiento();
            IEnumerable<SUCURSAL> lista = _Service.GetSucursales();

            return new SelectList(lista, "ID", "nombre", IDSucursal);
        } //comboBox de sucursales
        
        
        //
        //
        //====================================================================
        //======================== VISTAS PARCIALES ==========================
        //====================================================================
        //
        public PartialViewResult proveedorXproducto(int? id)
        {
            IEnumerable<PRODUCTOS> lista = null;
            ServiceMovimiento _Service = new ServiceMovimiento();
            if (id != null)
            {
                lista = _Service.GetProductosActivoXProveedor((int)id);
            }
            return PartialView("_ProductosGenerales", lista);
        } //Vista parcial con seleccion por proveedor
       
        public PartialViewResult sucursalXproducto(int? id)
        {
            IEnumerable<PRODUCTOS> lista = null;
            ServiceMovimiento _Service = new ServiceMovimiento();
            if (id != null)
            {
                lista = _Service.GetProductosActivoXSucursal((int)id);
            }
            ViewBag.SucursalSelecciona = id;
            return PartialView("_ProductosSucursal", lista);
        } //Vista parcial con seleccion por proveedor


        //
        //
        //====================================================================
        //========================== METODOS EXTRAS ==========================
        //====================================================================
        //
        private IEnumerable<PROVEEDORES> listaProveedores_lst(int IDProveedor = 0)
        {
            //Lista de Categorias
            ServiceProductos _ServiceProducto = new ServiceProductos();
            IEnumerable<PROVEEDORES> listaProveedores = _ServiceProducto.GetProveedores();
            return listaProveedores;
        } // En AgregarProducto del Modal (SIMULACION)

        private IEnumerable<PRODUCTOS> listaProductos_Filtrada(int? IDProveedor)
        {
            IEnumerable<PRODUCTOS> lista = null;
            ServiceMovimiento _Service = new ServiceMovimiento();
            if (IDProveedor != null)
            {
                lista = _Service.GetProductosActivoXProveedor((int)IDProveedor);
            }
            return lista;
        } // NO SE UTILIZA

        //
        //
        //====================================================================
        //=================== CONTROLLER DE VENTANAS =========================
        //====================================================================
        //
        public ActionResult MovimientoEntrada(int? idProveedor)
        {
            try
            {
                ServiceProductos _serviceProductos = new ServiceProductos();
                ViewModelMovimiento model = new ViewModelMovimiento();

                // ViewBag.ListaProveedores = listaProveedores_lst();
                ViewBag.IDProveedor = listaProveedores();
                ViewBag.IDSucursal = listaSucursales();

                model.prodList = (List<PRODUCTOS>)_serviceProductos.GetProductosActivo();
                if (TempData.ContainsKey("Notificacion"))
                {
                    ViewBag.NotificationMessage = TempData["Notificacion"];
                }
                
                return View(model);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData.Keep();
                return RedirectToAction("Default", "Error");
            }
        }

        public ActionResult MovimientoSalida(int? idSucursal)
        {
            try
            {
                ServiceProductos _serviceProductos = new ServiceProductos();
                ViewModelMovimiento model = new ViewModelMovimiento();

                ViewBag.IDProveedor = listaProveedores();
                ViewBag.IDSucursal = listaSucursales();

                model.prodList = (List<PRODUCTOS>)_serviceProductos.GetProductosActivo();

                return View(model);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData.Keep();
                return RedirectToAction("Default", "Error");
            }
        }

        public ActionResult MovimientoTransferencia()
        {
            return View();
        }



    }
}
