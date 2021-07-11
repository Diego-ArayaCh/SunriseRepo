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


        private SelectList listaProveedores(int IDCategoria = 0)
        {
            //Lista de Categorias
            ServiceProductos _ServiceProducto = new ServiceProductos();
            IEnumerable<PROVEEDORES> listaProveedores = _ServiceProducto.GetProveedores();

            return new SelectList(listaProveedores, "ID", "nombre", IDCategoria);
        }






        // GET: Movimiento
        public ActionResult MovimientoEntrada()
        {
            try
            {
                ViewBag.IDProveedor = listaProveedores();

                return View(new ViewModelMovimiento());
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
