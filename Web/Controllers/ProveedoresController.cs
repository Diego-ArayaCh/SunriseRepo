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
            }
            ViewBag.titulo = "Lista Productos";

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
                return RedirectToAction("IndexAdmin"); //preguntar si vamos usar la pagina "error"
            }
            return View(oPROVEEDORES);
        }
        catch (Exception ex)
        {
            Log.Error(ex, MethodBase.GetCurrentMethod());
            return RedirectToAction("Index");
        }
    }

        // GET: Proveedores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proveedores/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Proveedores/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Proveedores/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Proveedores/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
