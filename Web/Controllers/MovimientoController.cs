using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Security;
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
                    else
                    {
                        ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                 "Agregado",
                                                                 "El producto ha sido agregado",
                                                                 SweetAlertMessageType.success);
                    }
                    model = GestorBodega.Instancia.movimientoDetalle.historicoDetalle;
                }
                else
                {
                    ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                 "Error",
                                                                 "Indique la cantidad del producto a agregar",
                                                                 SweetAlertMessageType.error);
                }
                ViewBag.colspanDetalle = "6";
                //ViewBag.ListaProveedores = listaProveedores_lst();
                //ViewBag.IDProveedor = listaProveedores();
                return PartialView("_MovimientoDetalle", model);
            }
            catch (Exception ex)
            {
                 ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                 "Error",
                                                                 "Indique la cantidad del producto a agregar",
                                                                 SweetAlertMessageType.error);
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData.Keep();
                return RedirectToAction("Default", "Error");
            }

        }
        public ActionResult AgregarActualizarProductoSalida(int idSucursal, int cantidad, int id)
        {
            IEnumerable<HistDetalleEntradaSalida> model = new List<HistDetalleEntradaSalida>();
            ProdSuc prodSuc = new ServiceProductos().GetProductoSucursal(id , idSucursal);
            model = GestorBodega.Instancia.movimientoDetalle.historicoDetalle;
            if (cantidad > prodSuc.cant)
            {
                ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                 "Error",
                                                                 "La cantidad sobrepasa las existencias del producto en esta sucursal",
                                                                 SweetAlertMessageType.warning);
                model = GestorBodega.Instancia.movimientoDetalle.historicoDetalle;
                ViewBag.colspanDetalle = "5";
                return PartialView("_MovimientoDetalle", model);
            }
            try
            {
              
              
                if (cantidad > 0)
                {

                    bool validacion = GestorBodega.Instancia.AgregarActualizarSalida(new ServiceProductos().GetProductoByID(id), idSucursal, cantidad);
                    if (validacion)
                    {
                        TempData["Notificacion"] = Util.SweetAlertHelper.Mensaje(
                                                                   "Error",
                                                                   "El producto está agregado con otro proveedor",
                                                                   SweetAlertMessageType.warning);
                        TempData.Keep();
                        ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                   "Error",
                                                                   "El producto está agregado con otro proveedor",
                                                                   SweetAlertMessageType.warning);
                    }
                    else
                    {
                        ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                 "Agregado",
                                                                 "El producto ha sido agregado",
                                                                 SweetAlertMessageType.success);
                    }
                    model = GestorBodega.Instancia.movimientoDetalle.historicoDetalle;
                }
                else
                {
                    if (cantidad<0)
                    {
                        ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                 "Error",
                                                                 "Los números deben ser positivos",
                                                                 SweetAlertMessageType.error);
                        ViewBag.colspanDetalle = "5";
                        //ViewBag.ListaProveedores = listaProveedores_lst();
                        //ViewBag.IDProveedor = listaProveedores();
                        return PartialView("_MovimientoDetalle", model);
                    }
                    ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                 "Error",
                                                                 "Indique la cantidad del producto a agregar",
                                                                 SweetAlertMessageType.error);
                }
                ViewBag.colspanDetalle = "5";
                //ViewBag.ListaProveedores = listaProveedores_lst();
                //ViewBag.IDProveedor = listaProveedores();
                return PartialView("_MovimientoDetalle", model);
            }
            catch (Exception ex)
            {
                model = GestorBodega.Instancia.movimientoDetalle.historicoDetalle;
                ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                "Error",
                                                                "Indique la cantidad del producto a agregar",
                                                                SweetAlertMessageType.error);
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
               
                ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                   "Eliminado",
                                                                   "El producto se elimino del movimiento",
                                                                   SweetAlertMessageType.success);

                model = GestorBodega.Instancia.movimientoDetalle.historicoDetalle;
                int tipo = GestorBodega.Instancia.GetHistDetalle(idProd);
                
                if (tipo== 1)
                {
                    ViewBag.colspanDetalle = "6";
                }
                if (tipo==2)
                {
                    ViewBag.colspanDetalle = "5";
                }
                GestorBodega.Instancia.EliminarProducto(idProd);
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
        public ActionResult EjecutarMovimiento(int idSuc,string descripcion,int tipoMov)
        {
            IEnumerable<HistDetalleEntradaSalida> model = new List<HistDetalleEntradaSalida>();
            try
            {
                if (GestorBodega.Instancia.movimientoDetalle.historicoDetalle.Count == 0)
                {
                    ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                  "Error",
                                                                  "Detalle del movimiento vacío",
                                                                  SweetAlertMessageType.warning);
                    model = GestorBodega.Instancia.movimientoDetalle.historicoDetalle;
                    return PartialView("_MovimientoDetalle", model);
                }
               
                HISTORICO hist = new HISTORICO();
                hist.tipoMov = tipoMov;
               
                hist.IDUsuario = ((USUARIO) Session["User"]).ID;
                String fechaHora= DateTime.Now.ToString("dd/MM/yyyy hh:mmtt");

                try
                {
                    DateTime.ParseExact(fechaHora, "dd/MM/yyyy hh:mmtt", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    fechaHora=fechaHora.Replace(".", "");
                    if (fechaHora.Contains("p"))
                    {
                        fechaHora = fechaHora.Replace("p", "P");
                    }
                    else
                    {
                        fechaHora = fechaHora.Replace("a", "A");
                    }
                    String [] arrayFH=fechaHora.Split(' ');
                    String f1=arrayFH[0], f2=arrayFH[1];
                    fechaHora= f1 + " " + f2+"M";
                }
                hist.fechaHora = fechaHora;
                hist.detalle = descripcion;
                hist.HistDetalleEntradaSalida = GestorBodega.Instancia.movimientoDetalle.historicoDetalle;
                int tipo = 0;
                foreach (HistDetalleEntradaSalida h in hist.HistDetalleEntradaSalida)
                {
                     tipo = GestorBodega.Instancia.GetHistDetalle(h.IDProducto);
                    h.PRODUCTOS = null;
                    if (hist.tipoMov == 1) h.IDSucursalEntra = idSuc;
                    h.PROVEEDORES = null;
                }
                new ServiceMovimiento().GuardarMovimiento(hist);
                GestorBodega.Instancia.VaciarMovimiento();
                ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                   "Transacción",
                                                                   "El movimiento se ha ingresado en el sistema",
                                                                   SweetAlertMessageType.success);

                model = GestorBodega.Instancia.movimientoDetalle.historicoDetalle;
                

                if (tipo == 1)
                {
                    ViewBag.colspanDetalle = "6";
                }
                if (tipo == 2)
                {
                    ViewBag.colspanDetalle = "5";
                }
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
        private IEnumerable<SelectListItem> listaConceptoEntrada()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Selected = true,Text = "Seleccione el concepto", Value = ""},
                new SelectListItem{Text = "Compra", Value = "Compra"},
                new SelectListItem{Text = "Devolución", Value = "Devolución"},
                new SelectListItem{Text = "Ajuste inventario", Value = "Ajuste inventario"},
                new SelectListItem{Text = "Traspaso", Value = "Traspaso"},
                new SelectListItem{Text = "Inventario inicial", Value = "Inventario inicial"},

            };
            return items;
        }
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
       public PartialViewResult limpiarDetalle()
        {
            GestorBodega.Instancia.VaciarMovimiento();
            IEnumerable<HistDetalleEntradaSalida> model = GestorBodega.Instancia.movimientoDetalle.historicoDetalle;

            ViewBag.colspanDetalle = "5";
            return PartialView("_MovimientoDetalle",model);
        }
        public PartialViewResult sucursalXproducto(int? id)
        {
            IEnumerable<PRODUCTOS> lista = null;
            ServiceMovimiento _Service = new ServiceMovimiento();
            if (id != null)
            {
                lista = _Service.GetProductosActivoXSucursal((int)id);
            }
            if (GestorBodega.Instancia.movimientoDetalle.historicoDetalle.Count > 0)
            {
               
                ViewBag.Mensaje = Util.SweetAlertHelper.Mensaje(
                                                                  "Aviso",
                                                                  "Se ha eliminado el detalle del movimiento, solo se permite realizar movimientos de una sucursal a la vez",
                                                                  SweetAlertMessageType.warning);
            }
           
            ViewBag.SucursalSelecciona = id;
            ViewBag.colspanDetalle = "5";
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
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult MovimientoEntrada(int? idProveedor)
        {
            try
            {
                GestorBodega.Instancia.VaciarMovimiento();
            }
            catch (Exception)
            {

            }
            try
            {
               
                ServiceProductos _serviceProductos = new ServiceProductos();
                ViewModelMovimiento model = new ViewModelMovimiento();

                // ViewBag.ListaProveedores = listaProveedores_lst();
                ViewBag.IDProveedor = listaProveedores();
                ViewBag.IDSucursal = listaSucursales();
                ViewBag.conceptos = listaConceptoEntrada();
                ViewBag.usuario = ((USUARIO)Session["User"]).nombre+" "+ ((USUARIO)Session["User"]).apellidos;
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

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]

        public ActionResult MovimientoSalida(int? idSucursal)
        {
            try
            {
                GestorBodega.Instancia.VaciarMovimiento();
            }
            catch (Exception)
            {


            }
            try
            {
                
                ServiceProductos _serviceProductos = new ServiceProductos();
                ViewModelMovimiento model = new ViewModelMovimiento();

                ViewBag.IDProveedor = listaProveedores();
                ViewBag.IDSucursal = listaSucursales();
                ViewBag.usuario = ((USUARIO)Session["User"]).nombre + " " + ((USUARIO)Session["User"]).apellidos;
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

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult MovimientoTransferencia()
        {
            return View();
        }



    }
}
