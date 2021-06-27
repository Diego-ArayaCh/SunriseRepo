using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using Web.Utils;
using System.Diagnostics;

namespace Web.Controllers
{
    public class ProductosController : Controller
    {
        // GET: Productos
        public ActionResult IndexAdmin(int? page, string filtroBuscarProducto)
        {
            IEnumerable<PRODUCTOS> lista = null;
            try
            {
                ServiceProductos _ServiceProducto = new ServiceProductos();
                // Error porque viene en blanco 
                if (string.IsNullOrEmpty(filtroBuscarProducto))
                {
                    lista = _ServiceProducto.GetProductos();
                }
                else
                {
                    lista = _ServiceProducto.GetProductosxNombre(filtroBuscarProducto);
                }

                //Lista autocompletado de productos
                ViewBag.listaNombres = _ServiceProducto.GetProductoNombres();
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("IndexAdmin");
            }

            ViewBag.titulo = "Lista Productos";

            int pageSize = 5;
            int pageNumber = page ?? 1;
            return View(lista.ToPagedList(pageNumber, pageSize));

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



        private SelectList listaCategorias(int IDCategoria = 0)
        {
            //Lista de autores
            ServiceProductos _ServiceProducto = new ServiceProductos();
            IEnumerable<CATEGORIA> listaCategorias = _ServiceProducto.GetCategorias();
            //Autor SelectAutor = listaAutores.Where(c => c.IdAutor == idAutor).FirstOrDefault();

            //crea un combo box, los objetos son: lista, valor del objeto , y objeto a mostrar, y seleccionado
            return new SelectList(listaCategorias, "ID", "descripcion", IDCategoria);
        }
        private MultiSelectList listaSucursales(ICollection<SUCURSAL> sucursales)
        {
            //Lista de Categorias
            ServiceProductos _ServiceProducto = new ServiceProductos();
            IEnumerable<SUCURSAL> listaSucursales = _ServiceProducto.GetSucursales();
            int[] listaSucursalesSelect = null;

            if (sucursales != null)
            {

                listaSucursalesSelect = sucursales.Select(c => c.ID).ToArray();
            }

            return new MultiSelectList(listaSucursales, "ID", "nombre", listaSucursalesSelect);

        }
        private MultiSelectList listaProveedores(ICollection<PROVEEDORES> proveedores)
        {
            //Lista de Categorias
            ServiceProductos _ServiceProducto = new ServiceProductos();
            IEnumerable<PROVEEDORES> listaProveedores = _ServiceProducto.GetProveedores();
            int[] listaProveedoresSelect = null;

            if (proveedores != null)
            {
                listaProveedoresSelect = proveedores.Select(c => c.ID).ToArray();
            }

            return new MultiSelectList(listaProveedores, "ID", "nombre", listaProveedoresSelect);

        }



        // GET: Libro/Create
        public ActionResult Create()
        {
            //Lista de autores
            ViewBag.IdCategoria = listaCategorias();
            ViewBag.IdSucursal = listaSucursales(null);
            ViewBag.IdProveedor = listaProveedores(null);

            return View();
        }

        // POST: Libro/Edit/5
        [HttpPost]
        public ActionResult Save(PRODUCTOS oProducto, HttpPostedFileBase ImageFile, string[] selectedSucursales, string[] selectedProveedores)
        {

            ServiceProductos _ServiceProducto = new ServiceProductos();
            try
            {
                if (oProducto.imagen == null)
                {
                    if (ImageFile != null)
                    {
                        String pathImagen = ImageFile.FileName;

                      //  string sourcePath = @"C:\Users\Public\TestFolder";
                        string targetPath = @"/Content/IMAGES-PRODUCTS/";

                        // Use Path class to manipulate file and directory paths.
                        string sourceFile = pathImagen;
                        string destFile = System.IO.Path.Combine(targetPath, pathImagen);

                        // To copy a file to another location and
                        // overwrite the destination file if it already exists.
                        System.IO.File.Copy(sourceFile, destFile, true);

                        oProducto.imagen = "/Content/IMAGES-PRODUCTS/"+pathImagen;
                        ModelState.Remove("imagen");
                    }

                }
                if (ModelState.IsValid)
                {
                    PRODUCTOS oProductoI = _ServiceProducto.Save(oProducto, selectedSucursales, selectedProveedores);
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Util.ValidateErrors(this);

                    //ViewBag.IdCategoria = listaCategorias(oProducto.IDCategoria);
                    //ViewBag.IdSucursal = listaSucursales(oProducto.ProdSuc);
                    //ViewBag.IdProveedor = listaProveedores(null);

                    ViewBag.IdCategoria = listaCategorias();
                    ViewBag.IdSucursal = listaSucursales(null);
                    ViewBag.IdProveedor = listaProveedores(null);

                    return View("Create", oProducto);
                }


                return RedirectToAction("IndexAdmin");

            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Producto";
                TempData["Redirect-Action"] = "IndexAdmin";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }





        // GET: Libro/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                return View();

            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Libro";
                TempData["Redirect-Action"] = "IndexAdmin";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }


    }
}
