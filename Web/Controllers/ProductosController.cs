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
using System.IO;

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
        public ActionResult Save(PRODUCTOS oProducto, HttpPostedFileBase ImageFile, string[] selectedProveedores)
        {
            MemoryStream target = new MemoryStream();
            ServiceProductos _ServiceProducto = new ServiceProductos();
            try
            {
                if (oProducto.imagen == null)
                {
                    if (ImageFile != null)
                    {
                        ImageFile.InputStream.CopyTo(target);
                        oProducto.imagen = target.ToArray();
                        ModelState.Remove("imagen");
                    }

                }
                if (ModelState.IsValid)
                {
                    PRODUCTOS oProductoI = _ServiceProducto.Save(oProducto, selectedProveedores);
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Util.ValidateErrors(this);

                    //ViewBag.IdCategoria = listaCategorias(oProducto.IDCategoria);
                    //ViewBag.IdSucursal = listaSucursales(oProducto.ProdSuc);
                    //ViewBag.IdProveedor = listaProveedores(null);

                    ViewBag.IdCategoria = listaCategorias();
                    //ViewBag.IdSucursal = listaSucursales(null);
                    ViewBag.IdProveedor = listaProveedores(oProducto.PROVEEDORES);

                    return View("Create", oProducto);
                }


                return RedirectToAction("IndexAdmin");

            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Productos";
                TempData["Redirect-Action"] = "IndexAdmin";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        [HttpPost]
        public ActionResult Save2(PRODUCTOS oProducto, HttpPostedFileBase ImageFile, string[] selectedProveedores)
        {
            MemoryStream target = new MemoryStream();
            ServiceProductos _ServiceProducto = new ServiceProductos();
            try
            {
                if (oProducto.imagen == null)
                {
                    if (ImageFile != null)
                    {
                        ImageFile.InputStream.CopyTo(target);
                        oProducto.imagen = target.ToArray();
                        ModelState.Remove("imagen");
                    }

                }
                if (ModelState.IsValid)
                {
                    PRODUCTOS oProductoI = _ServiceProducto.Save(oProducto, selectedProveedores);
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Util.ValidateErrors(this);
                    ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oProducto.IDCategoria));
                    ViewBag.IdProveedor = listaProveedores(oProducto.PROVEEDORES);

                    return View("Edit", oProducto);
                }


                return RedirectToAction("IndexAdmin");

            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Productos";
                TempData["Redirect-Action"] = "IndexAdmin";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }


        // GET: Libro/Edit/5
        public ActionResult Edit(int? id)
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
                    TempData["Message"] = "No existe el producto solicitado";
                    TempData["Redirect"] = "Productos";
                    TempData["Redirect-Action"] = "IndexAdmin";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                
                //Lista de autores
                //List<SUCURSAL> listaSucursalesAux = new List<SUCURSAL>();
                //foreach (var item in oPRODUCTOS.ProdSuc)
                //{
                //    listaSucursalesAux.Append(_ServiceProducto.GetSucursalesByID(item.IDSucursal));
                //}

                ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oPRODUCTOS.IDCategoria));
                //ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);
                ViewBag.IdProveedor = listaProveedores(oPRODUCTOS.PROVEEDORES);

                return View(oPRODUCTOS);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Productos";
                TempData["Redirect-Action"] = "IndexAdmin";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }




    }
}
