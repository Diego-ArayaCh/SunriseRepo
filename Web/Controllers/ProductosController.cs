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
using Web.Security;
using Web.Util;

namespace Web.Controllers
{
    public class ProductosController : Controller
    {
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
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
                    ViewBag.Filtro = "";
                }
                else
                {
                    lista = _ServiceProducto.GetProductosxNombre(filtroBuscarProducto);
                    ViewBag.Filtro = filtroBuscarProducto;
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

            if (TempData.ContainsKey("Notificacion_Guardar"))
            {
                ViewBag.NotificationMessage = TempData["Notificacion_Guardar"];
            }
            if (TempData.ContainsKey("Notificacion_Editar"))
            {
                ViewBag.NotificationMessage = TempData["Notificacion_Editar"];
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;
            return View(lista.ToPagedList(pageNumber, pageSize));

        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
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

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult Inventario(int? page, string filtroBuscarProducto)
        {
            IEnumerable<PRODUCTOS> lista = null;
            IEnumerable<PRODUCTOS> listaActivos = new List<PRODUCTOS>();
            try
            {
                ServiceProductos _ServiceProducto = new ServiceProductos();
                // Error porque viene en blanco 
                if (string.IsNullOrEmpty(filtroBuscarProducto))
                {
                    lista = _ServiceProducto.GetProductosActivo();
                    ViewBag.Filtro = "";
                }
                else
                {
                    lista = _ServiceProducto.GetProductosxNombreActivo(filtroBuscarProducto);
                    ViewBag.Filtro = filtroBuscarProducto;
                }

                //Lista autocompletado de productos
                ViewBag.listaNombres = _ServiceProducto.GetProductoNombres();
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("Inventario");
            }

            ViewBag.titulo = "Lista Inventario";

            int pageSize = 6;
            int pageNumber = page ?? 1;
            return View(lista.ToPagedList(pageNumber, pageSize));
        }

        //
        //
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
        public IEnumerable<SelectListItem> listaEstados(int seleccionado)
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "Activo", Value = "1"},
                new SelectListItem{Text = "Inactivo", Value = "2"},
            };
            foreach (var item in items)
            {
                if (item.Value == seleccionado+"")
                {
                    item.Selected = true;
                    break;
                }
            }

            return items;
        }
        //
        //
        //=================================================================================================================================================
        //=========< < M O D E L O - N U M E R O [1] > >===================================================================================================
        //=================================================================================================================================================
        //METODOS QUE TRABAJAN **SIN** LAS SUCURSALES
        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Create()
        {
            //Lista de autores
            ViewBag.IdCategoria = listaCategorias();
            ViewBag.IdSucursal = listaSucursales(null);
            ViewBag.IdProveedor = listaProveedores(null);

            return View();
        }

        [CustomAuthorize((int)Roles.Administrador)]
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
                ViewBag.estado = listaEstados(Convert.ToInt32(oPRODUCTOS.estado));

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

        [HttpPost] //CREAR
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
                    oProducto.estado = 1;
                    oProducto.stock = 0;
                    PRODUCTOS oProductoI = _ServiceProducto.Save(oProducto, selectedProveedores);
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Web.Utils.Util.ValidateErrors(this);
                    ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oProducto.IDCategoria));
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

        [HttpPost] //EDITAR
        public ActionResult Save2(PRODUCTOS oProducto, HttpPostedFileBase ImageFile, string[] selectedProveedores, string estado)
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
                if (oProducto.imagen != null)
                {
                    if (ImageFile != null)
                    {
                        ImageFile.InputStream.CopyTo(target);
                        if (oProducto.imagen != target.ToArray())
                        {
                            oProducto.imagen = target.ToArray();
                        }
                    }
                }

                oProducto.estado= estado.Equals("1")? 1 :  2;

                if (ModelState.IsValid)
                {
                    PRODUCTOS oProductoI = _ServiceProducto.Save(oProducto, selectedProveedores);
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Web.Utils.Util.ValidateErrors(this);
                    ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oProducto.IDCategoria));
                    ViewBag.IdProveedor = listaProveedores(oProducto.PROVEEDORES);
                    ViewBag.estado = listaEstados(Convert.ToInt32(oProducto.estado));

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

        //
        //
        //=================================================================================================================================================
        //=========< < M O D E L O - N U M E R O [2] > >===================================================================================================
        //=================================================================================================================================================
        //METODOS QUE TRABAJAN CON LAS SUCURSALES
        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Create2()
        {
            //Lista de autores
            ViewBag.IdCategoria = listaCategorias();
            ViewBag.IdSucursal = listaSucursales(null);
            ViewBag.IdProveedor = listaProveedores(null);

            return View();
        }

        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Edit2(int? id)
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
                ICollection<SUCURSAL> listaSucursalesAux = new List<SUCURSAL>();
                foreach (var item in oPRODUCTOS.ProdSuc)
                {
                    listaSucursalesAux.Add(_ServiceProducto.GetSucursalesByID(item.IDSucursal));
                }
                ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);

                ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oPRODUCTOS.IDCategoria));
                ViewBag.IdProveedor = listaProveedores(oPRODUCTOS.PROVEEDORES);
                ViewBag.estado = listaEstados(Convert.ToInt32(oPRODUCTOS.estado));

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

        [HttpPost] //CREAR
        public ActionResult Save_AUX(PRODUCTOS oProducto, HttpPostedFileBase ImageFile, string[] selectedSucursales, string[] selectedProveedores)
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
                    else
                    {
                        ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje(
                           "Atención",
                           "No hay imagen seleccionada",
                           SweetAlertMessageType.error);

                        // Valida Errores si Javascript está deshabilitado
                        Web.Utils.Util.ValidateErrors(this);
                        ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oProducto.IDCategoria));
                        ViewBag.IdProveedor = listaProveedores(oProducto.PROVEEDORES);

                        ICollection<SUCURSAL> listaSucursalesAux = new List<SUCURSAL>();
                        foreach (var item in oProducto.ProdSuc)
                        {
                            listaSucursalesAux.Add(_ServiceProducto.GetSucursalesByID(item.IDSucursal));
                        }
                        ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);

                        return View("Create2", oProducto);
                    }

                }
                if (oProducto.imagen != null)
                {
                    if (ImageFile != null)
                    {
                        ImageFile.InputStream.CopyTo(target);
                        if (oProducto.imagen != target.ToArray())
                        {
                            oProducto.imagen = target.ToArray();
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    if (selectedSucursales == null)
                    {
                        ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje(
                            "Atención",
                            "No hay sucursal(es) seleccionada(s)",
                            SweetAlertMessageType.error);

                        // Valida Errores si Javascript está deshabilitado
                        Web.Utils.Util.ValidateErrors(this);
                        ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oProducto.IDCategoria));
                        ViewBag.IdProveedor = listaProveedores(oProducto.PROVEEDORES);

                        ICollection<SUCURSAL> listaSucursalesAux = new List<SUCURSAL>();
                        foreach (var item in oProducto.ProdSuc)
                        {
                            listaSucursalesAux.Add(_ServiceProducto.GetSucursalesByID(item.IDSucursal));
                        }
                        ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);

                        return View("Create2", oProducto);
                    }

                    if (selectedProveedores == null)
                    {
                        ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje(
                            "Atención",
                            "No hay proveedor(es) seleccionado(s)",
                            SweetAlertMessageType.error);

                        // Valida Errores si Javascript está deshabilitado
                        Web.Utils.Util.ValidateErrors(this);
                        ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oProducto.IDCategoria));
                        ViewBag.IdProveedor = listaProveedores(oProducto.PROVEEDORES);

                        ICollection<SUCURSAL> listaSucursalesAux = new List<SUCURSAL>();
                        foreach (var item in oProducto.ProdSuc)
                        {
                            listaSucursalesAux.Add(_ServiceProducto.GetSucursalesByID(item.IDSucursal));
                        }
                        ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);

                        return View("Create2", oProducto);
                    }

                    oProducto.estado = 2;
                    oProducto.stock = 0;
                    PRODUCTOS oProductoI = _ServiceProducto.Save_AUX(oProducto, selectedSucursales, selectedProveedores);
                    TempData["Notificacion_Guardar"] = Util.SweetAlertHelper.Mensaje(
                                                                    "Registro", 
                                                                    "Exito al guardar el producto", 
                                                                    SweetAlertMessageType.success);
                    TempData.Keep();
                    //ViewBag.NotificationMessage
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Web.Utils.Util.ValidateErrors(this);
                    ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oProducto.IDCategoria));
                    ViewBag.IdProveedor = listaProveedores(oProducto.PROVEEDORES);

                    ICollection<SUCURSAL> listaSucursalesAux = new List<SUCURSAL>();
                    foreach (var item in oProducto.ProdSuc)
                    {
                        listaSucursalesAux.Add(_ServiceProducto.GetSucursalesByID(item.IDSucursal));
                    }
                    ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);

                    return View("Create2", oProducto);
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

        [HttpPost] //EDITAR
        public ActionResult Save2_AUX(PRODUCTOS oProducto, HttpPostedFileBase ImageFile, string[] selectedSucursales, string[] selectedProveedores, string estado)
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
                    else
                    {
                        ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje(
                           "Atención",
                           "No hay imagen seleccionada",
                           SweetAlertMessageType.error);

                        // Valida Errores si Javascript está deshabilitado
                        Web.Utils.Util.ValidateErrors(this);
                        ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oProducto.IDCategoria));
                        ViewBag.IdProveedor = listaProveedores(oProducto.PROVEEDORES);

                        ICollection<SUCURSAL> listaSucursalesAux = new List<SUCURSAL>();
                        foreach (var item in oProducto.ProdSuc)
                        {
                            listaSucursalesAux.Add(_ServiceProducto.GetSucursalesByID(item.IDSucursal));
                        }
                        ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);

                        return View("Edit2", oProducto);
                    }

                }
                if (oProducto.imagen != null)
                {
                    if (ImageFile != null)
                    {
                        ImageFile.InputStream.CopyTo(target);
                        if (oProducto.imagen != target.ToArray())
                        {
                            oProducto.imagen = target.ToArray();
                        }
                    }
                }

                oProducto.estado = estado.Equals("1") ? 1 : 2;

                if (ModelState.IsValid)
                {

                    if (selectedSucursales == null)
                    {
                        ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje(
                            "Atención",
                            "No hay sucursal(es) seleccionada(s)",
                            SweetAlertMessageType.error);

                        // Lista de autores
                        ICollection<SUCURSAL> listaSucursalesAux = new List<SUCURSAL>();
                        foreach (var item in oProducto.ProdSuc)
                        {
                            listaSucursalesAux.Append(_ServiceProducto.GetSucursalesByID(item.IDSucursal));
                        }
                        ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);

                        ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oProducto.IDCategoria));
                        ViewBag.IdProveedor = listaProveedores(oProducto.PROVEEDORES);
                        ViewBag.estado = listaEstados(Convert.ToInt32(oProducto.estado));

                        return View("Edit2", oProducto);
                    }

                    if (selectedProveedores == null)
                    {
                        ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje(
                            "Atención",
                            "No hay proveedor(es) seleccionado(s)",
                            SweetAlertMessageType.error);

                        // Lista de autores
                        ICollection<SUCURSAL> listaSucursalesAux = new List<SUCURSAL>();
                        foreach (var item in oProducto.ProdSuc)
                        {
                            listaSucursalesAux.Append(_ServiceProducto.GetSucursalesByID(item.IDSucursal));
                        }
                        ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);

                        ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oProducto.IDCategoria));
                        ViewBag.IdProveedor = listaProveedores(oProducto.PROVEEDORES);
                        ViewBag.estado = listaEstados(Convert.ToInt32(oProducto.estado));

                        return View("Edit2", oProducto);
                    }

                    PRODUCTOS oProductoI = _ServiceProducto.Save_AUX(oProducto, selectedSucursales, selectedProveedores);
                    TempData["Notificacion_Editar"] = Util.SweetAlertHelper.Mensaje(
                                                                  "Registro",
                                                                  "Exito al actualizar el producto",
                                                                  SweetAlertMessageType.success);
                    TempData.Keep();
                }
                else
                {
                    // Lista de autores
                    ICollection<SUCURSAL> listaSucursalesAux = new List<SUCURSAL>();
                    foreach (var item in oProducto.ProdSuc)
                    {
                        listaSucursalesAux.Append(_ServiceProducto.GetSucursalesByID(item.IDSucursal));
                    }
                    ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);

                    ViewBag.IdCategoria = listaCategorias(Convert.ToInt32(oProducto.IDCategoria));
                    ViewBag.IdProveedor = listaProveedores(oProducto.PROVEEDORES);
                    ViewBag.estado = listaEstados(Convert.ToInt32(oProducto.estado));

                    return View("Edit2", oProducto);
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

    }
}
