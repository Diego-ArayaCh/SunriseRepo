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
            return View(oPRODUCTOS);
        }
        catch (Exception ex)
        {
            Log.Error(ex, MethodBase.GetCurrentMethod());
            return RedirectToAction("Index");
        }
    }

    // GET: Productos/Create
    public ActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
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

        // GET: Productos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Productos/Edit/5
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

        // GET: Productos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Productos/Delete/5
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
