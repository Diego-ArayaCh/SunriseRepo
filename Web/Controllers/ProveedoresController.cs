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
    public class ProveedoresController : Controller
    {
        // GET: Proveedores
        public ActionResult IndexAdmin()
        {
            IEnumerable<PROVEEDORES> lista = null;
            try
            {
                ServiceProveedores _ServiceProveedores = new ServiceProveedores();

                lista = _ServiceProveedores.GetProveedores();
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 

                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("IndexAdmin");
            }

            ViewBag.titulo = "Lista Proveedores";
            return View(lista);
        }


        // GET: Proveedores/Details/5
        public ActionResult Details(int? id)
        {
            ServiceProveedores _ServiceProveedor = new ServiceProveedores();
            PROVEEDORES oPROVEEDORES = null;
            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("IndexAdmin");
                }

                oPROVEEDORES = _ServiceProveedor.GetProveedorByID(id.Value);
                if (oPROVEEDORES == null)
                {
                    return RedirectToAction("IndexAdmin");
                }
                ViewBag.titulo = "Detalle Proveedor";
                return View(oPROVEEDORES);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("IndexAdmin");
            }
        }

       
    }
}
