using ApplicationCore.Services;
using Infraestructure.Models;
using PagedList;
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
        //[CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult IndexAdmin(int? page, string filtroBuscarProveedor)
        {
            IEnumerable<PROVEEDORES> lista = null;
            try
            {
                ServiceProveedores _ServiceProveedores = new ServiceProveedores();
                if (string.IsNullOrEmpty(filtroBuscarProveedor))
                {
                    lista = _ServiceProveedores.GetProveedores();
                }
                else
                {
                    lista = _ServiceProveedores.GetProductosxNombre(filtroBuscarProveedor);
                }
                //Lista autocompletado de productos
                ViewBag.listaNombres = _ServiceProveedores.GetProveedoresNombre();
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 

                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("IndexAdmin");
            }

            ViewBag.titulo = "Lista Proveedores";
            int pageSize = 5;
            int pageNumber = page ?? 1;
            return View(lista.ToPagedList(pageNumber, pageSize));
          

        }


        // GET: Proveedores/Details/5
        //[CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
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
        public ActionResult Create()
        {
            //Lista de autores

            ViewBag.IDPais = listaPaises();
            ViewBag.Contacto = listaContactos(null);

            return View();
        }
        public ActionResult Edit(int? id)
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

                ViewBag.IDPais = listaPaises(Convert.ToInt32(oPROVEEDORES.IDPais));
                //ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);
                ViewBag.selectedContactos = listaContactos(oPROVEEDORES.ID);
                ViewBag.estado = listaEstados(Convert.ToInt32(oPROVEEDORES.estado));

                return View(oPROVEEDORES);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Proveedores";
                TempData["Redirect-Action"] = "IndexAdmin";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }
        private SelectList listaPaises(int IDPais = 0)
        {
            //Lista de autores
            ServiceProveedores _ServiceProveedor = new ServiceProveedores();
            IEnumerable<PAIS> listaPaises = _ServiceProveedor.GetPaises();
            //Autor SelectAutor = listaAutores.Where(c => c.IdAutor == idAutor).FirstOrDefault();

            //crea un combo box, los objetos son: lista, valor del objeto , y objeto a mostrar, y seleccionado
            return new SelectList(listaPaises, "ID", "nombre", IDPais);
        }
        private SelectList listaContactos(int? id)
        {
            //Lista de autores
            ServiceProveedores _ServiceProveedor = new ServiceProveedores();
            IEnumerable<CONTACTO> selectedContactos = _ServiceProveedor.GetContactosByProveedorID(id);
            //Autor SelectAutor = listaAutores.Where(c => c.IdAutor == idAutor).FirstOrDefault();

            //crea un combo box, los objetos son: lista, valor del objeto , y objeto a mostrar, y seleccionado
            return new SelectList(selectedContactos, "ID", "nombre");


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
                if (item.Value == seleccionado + "")
                {
                    item.Selected = true;
                    break;
                }
            }

            return items;
        }
        [HttpPost]
        public ActionResult Save(PROVEEDORES oProveedores,List<CONTACTO> selectedContactos)
        {

            ServiceProveedores serviceProveedores = new ServiceProveedores();
            try
            {

                if (ModelState.IsValid)
                {
                    oProveedores.estado = 1;

                    PROVEEDORES oProveedores1 = serviceProveedores.Save(oProveedores, selectedContactos);
                }
                else
                {
                    // Valida Errores si Javascript está deshabilitado
                    Web.Utils.Util.ValidateErrors(this);
                    ViewBag.IDPais = listaPaises(Convert.ToInt32(oProveedores.IDPais));
                    //ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);

                    ViewBag.estado = listaEstados(Convert.ToInt32(oProveedores.estado));

                    return View("Create", oProveedores);
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
