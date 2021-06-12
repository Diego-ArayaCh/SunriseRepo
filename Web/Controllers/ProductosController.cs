using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ProductosController : Controller
    {
        // GET: Productos
        public ActionResult IndexAdmin()
        {
            IEnumerable<PRODUCTOS> lista = null;
            try
            {
                ServiceProductos _ServiceProducto = new ServiceProductos();

                lista = _ServiceProducto.GetProductos();
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("IndexAdmin");
            }

            ViewBag.titulo = "Lista Productos";
            return View(lista);

        }


        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {
            ServiceProductos _ServiceProducto = new ServiceProductos();
            PRODUCTOS oPRODUCTOS = null;
            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("IndexAdmin");
                }

                oPRODUCTOS = _ServiceProducto.GetProductoByID(id.Value);
                if (oPRODUCTOS == null)
                {
                    return RedirectToAction("IndexAdmin"); //preguntar si vamos usar la pagina "error"
                }

                ViewBag.titulo = "Detalle Producto";
                return View(oPRODUCTOS);

            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("IndexAdmin");
            }
        }


    }
}
