using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.ViewModel;

namespace Web.Controllers
{
    public class MovimientoController : Controller
    {
       [HttpPost]
        public ActionResult AgregarProducto(int cantidad, int id)
        {
            ViewModelMovimiento model = new ViewModelMovimiento();
            model.prodListDetalle.Add(new ServiceProductos().GetProductoByID(id));
            model.prodList = new ServiceProductos().GetProductos().ToList();
            try
            {
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




        private SelectList listaProveedores(int IDCategoria = 0)
        {
            //Lista de Categorias
            ServiceProductos _ServiceProducto = new ServiceProductos();
            IEnumerable<PROVEEDORES> listaProveedores = _ServiceProducto.GetProveedores();

            return new SelectList(listaProveedores, "ID", "nombre", IDCategoria);
        }
        private IEnumerable<PROVEEDORES> listaProveedores_lst(int IDCategoria = 0)
        {
            //Lista de Categorias
            ServiceProductos _ServiceProducto = new ServiceProductos();
            IEnumerable<PROVEEDORES> listaProveedores = _ServiceProducto.GetProveedores();
            return listaProveedores;
        }

        public PartialViewResult proveedorXproducto(int? id)
        {
            IEnumerable<PRODUCTOS> lista = null;
            ServiceMovimiento _Service = new ServiceMovimiento();
            if (id != null)
            {
                lista = _Service.GetProductosActivoXProveedor((int)id);
            }
            return PartialView("_ProductosGenerales", lista);
        }






        // GET: Movimiento
        public ActionResult MovimientoEntrada()
        {
            try
            {
                ViewBag.ListaProveedores = listaProveedores_lst();
                ViewBag.IDProveedor = listaProveedores();
                ViewModelMovimiento model = new ViewModelMovimiento();
               // model.prodList = new ServiceProductos().GetProductos().ToList();
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

        public ActionResult MovimientoSalida()
        {
            return View();
        }

        public ActionResult MovimientoTransferencia()
        {
            return View();
        }



    }
}
