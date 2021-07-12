using ApplicationCore.Services;
using Infraestructure.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Web.Security;
using Web.ViewModel;


using Web.Utils;
using System.Diagnostics;
using Web.Util;

namespace Web.Controllers
{
   
    public class ProveedoresController : Controller
    {
        #region Acciones normales
       
        // GET: Proveedores
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult IndexAdmin(int? page, string filtroBuscarProveedor)
        {
            IEnumerable<PROVEEDORES> lista = null;
            try
            {
                ServiceProveedores _ServiceProveedores = new ServiceProveedores();
                if (string.IsNullOrEmpty(filtroBuscarProveedor))
                {
                    lista = _ServiceProveedores.GetProveedores();
                    ViewBag.Filtro = "";
                }
                else
                {
                    lista = _ServiceProveedores.GetProductosxNombre(filtroBuscarProveedor);
                    ViewBag.Filtro = filtroBuscarProveedor;
                }
                //Lista autocompletado de productos
                ViewBag.listaNombres = _ServiceProveedores.GetProveedoresNombre();
                if (TempData.ContainsKey("Notificacion_Guardar"))
                {
                    ViewBag.NotificationMessage = TempData["Notificacion_Guardar"];
                }
                if (TempData.ContainsKey("Notificacion_Editar"))
                {
                    ViewBag.NotificationMessage = TempData["Notificacion_Editar"];
                }
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
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
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
        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Create()
        {
            //Lista de autores
            ViewBag.BackgroundColor = "white";
            ViewBag.pais = listaPaises();
            ViewBag.estado = listaEstados(1);
            CrudContactos.Instancia.Items.Clear();
            ViewBag.contactos = CrudContactos.Instancia.Items;
            return View();
        }
        [CustomAuthorize((int)Roles.Administrador)]
        public ActionResult Edit(int? id)
        {
            CrudContactos.Instancia.Items.Clear();
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
                ViewBag.estado = listaEstados(Convert.ToInt32(oPROVEEDORES.estado));
                ViewBag.pais = listaPaises(Convert.ToInt32(oPROVEEDORES.IDPais));
                //ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);
                foreach (var item in listaContactos(oPROVEEDORES.ID))
                {
                    ViewModelContactos oViewModelContactos = new ViewModelContactos();
                    oViewModelContactos.ID = item.ID;
                    oViewModelContactos.IDProv = item.IDProv;
                    oViewModelContactos.nombre = item.nombre;
                    oViewModelContactos.telefono = item.telefono;
                    oViewModelContactos.correo = item.correo;
                    oViewModelContactos.contacto = item;
                    CrudContactos.Instancia.GuardarContacto(oViewModelContactos);
                }
                ViewBag.contactos = CrudContactos.Instancia.Items;
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


        [HttpPost]
        public ActionResult Save(PROVEEDORES pProveedores)
        {
            List<CONTACTO> contactos = new List<CONTACTO>();
            ServiceProveedores serviceProveedores = new ServiceProveedores();
            try
            {
                ServiceProveedores _service = new ServiceProveedores();
                PROVEEDORES oProveedor = new PROVEEDORES();
                oProveedor = _service.GetProveedorByID(pProveedores.ID);
                if (oProveedor == null)
                {
                    pProveedores.estado = 1;
                    if (!ModelState.IsValid)
                    {

                        ViewBag.pais = listaPaises(Convert.ToInt32(pProveedores.IDPais));
                        ViewBag.estado = listaEstados(Convert.ToInt32(pProveedores.estado));
                        ViewBag.contactos = CrudContactos.Instancia.Items;
                        ViewBag.NotificationMessage = SweetAlertHelper.Mensaje("Error", "Hay campos sin llenar", SweetAlertMessageType.error);
                        return View("Create", pProveedores);

                    }
                    if (ValidarListaContactos(pProveedores) && pProveedores.IDPais==null)
                    {
                        ViewBag.pais = listaPaises(Convert.ToInt32(pProveedores.IDPais));
                        ViewBag.estado = listaEstados(Convert.ToInt32(pProveedores.estado));
                        ViewBag.contactos = CrudContactos.Instancia.Items;
                        ViewBag.NotificationMessage = SweetAlertHelper.Mensaje("Error", "Seleccione un país y registre contactos", SweetAlertMessageType.error);
                        return View("Create", pProveedores);
                    }


                   
                    if (pProveedores.IDPais==null)
                    {
                        ViewBag.pais = listaPaises(Convert.ToInt32(pProveedores.IDPais));
                        ViewBag.estado = listaEstados(Convert.ToInt32(pProveedores.estado));
                        ViewBag.contactos = CrudContactos.Instancia.Items;
                        ViewBag.NotificationMessage = SweetAlertHelper.Mensaje("Error", "Seleccione un país", SweetAlertMessageType.error);
                        return View("Create", pProveedores);
                    }
                    if (ValidarListaContactos(pProveedores))
                    {
                        
                       
                        ViewBag.pais = listaPaises(Convert.ToInt32(pProveedores.IDPais));
                        ViewBag.estado = listaEstados(Convert.ToInt32(pProveedores.estado));
                        ViewBag.contactos = CrudContactos.Instancia.Items;
                        ViewBag.NotificationMessage = SweetAlertHelper.Mensaje("Error","No hay contactos registrados",SweetAlertMessageType.error);
                        return View("Create", pProveedores);
                    }
                    else
                    {
                       
                        foreach (var item in CrudContactos.Instancia.Items)
                        {
                            CONTACTO oContacto = new CONTACTO();
                            oContacto.correo = item.correo;
                            oContacto.estado = 1;
                            oContacto.nombre = item.nombre;
                            oContacto.telefono = item.telefono;
                           contactos.Add(oContacto);
                        }
                    }
                    if (ModelState.IsValid)
                    {


                        PROVEEDORES oProveedores1 = serviceProveedores.Save(pProveedores,contactos);
                        TempData["Notificacion_Guardar"] = Util.SweetAlertHelper.Mensaje(
                                                                   "Registrado",
                                                                   "Exito al guardar el contacto",
                                                                   SweetAlertMessageType.success);
                        TempData.Keep();
                    }
                    else
                    {
                        // Valida Errores si Javascript está deshabilitado
                        Web.Utils.Util.ValidateErrors(this);
                        ViewBag.pais = listaPaises(Convert.ToInt32(pProveedores.IDPais));
                        //ViewBag.IdSucursal = listaSucursales(listaSucursalesAux);

                        ViewBag.estado = listaEstados(Convert.ToInt32(pProveedores.estado));

                        return View("Create", pProveedores);
                    }
                }
                else
                {
                    
                    if (!ModelState.IsValid)
                    {

                        ViewBag.pais = listaPaises(Convert.ToInt32(pProveedores.IDPais));
                        ViewBag.estado = listaEstados(Convert.ToInt32(pProveedores.estado));
                        ViewBag.contactos = CrudContactos.Instancia.Items;
                        ViewBag.NotificationMessage = SweetAlertHelper.Mensaje("Error", "Hay campos sin llenar", SweetAlertMessageType.error);
                        return View("Edit", pProveedores);

                    }
                    if (ValidarListaContactos(pProveedores) && pProveedores.IDPais == null)
                    {
                        ViewBag.pais = listaPaises(Convert.ToInt32(pProveedores.IDPais));
                        ViewBag.estado = listaEstados(Convert.ToInt32(pProveedores.estado));
                        ViewBag.contactos = CrudContactos.Instancia.Items;
                        ViewBag.NotificationMessage = SweetAlertHelper.Mensaje("Error", "Seleccione un país y registre contactos", SweetAlertMessageType.error);
                        return View("Edit", pProveedores);
                    }


                   
                    if (pProveedores.IDPais == null)
                    {
                        ViewBag.pais = listaPaises(Convert.ToInt32(pProveedores.IDPais));
                        ViewBag.estado = listaEstados(Convert.ToInt32(pProveedores.estado));
                        ViewBag.contactos = CrudContactos.Instancia.Items;
                        ViewBag.NotificationMessage = SweetAlertHelper.Mensaje("Error", "Seleccione un país", SweetAlertMessageType.error);
                        return View("Edit", pProveedores);
                    }
                    if (ValidarListaContactos(pProveedores))
                    {


                        ViewBag.pais = listaPaises(Convert.ToInt32(pProveedores.IDPais));
                        ViewBag.estado = listaEstados(Convert.ToInt32(pProveedores.estado));
                        ViewBag.contactos = CrudContactos.Instancia.Items;
                        ViewBag.NotificationMessage = SweetAlertHelper.Mensaje("Error", "No hay contactos registrados", SweetAlertMessageType.error);
                        return View("Edit", pProveedores);
                    }
                    else
                    {
                        foreach (var item in CrudContactos.Instancia.Items)
                        {
                            CONTACTO oContacto = new CONTACTO();
                            oContacto.ID = item.ID;
                            oContacto.IDProv = item.IDProv;
                            oContacto.nombre = item.nombre;
                            oContacto.telefono = item.telefono;
                            oContacto.correo = item.correo;
                            contactos.Add(oContacto);

                        }
                    }

                    



                    


                    if (ModelState.IsValid)
                    {

                        
                        PROVEEDORES oProveedores1 = serviceProveedores.Save(pProveedores, contactos);
                        TempData["Notificacion_Guardar"] = Util.SweetAlertHelper.Mensaje(
                                                                     "Actualizado",
                                                                     "Exito al actualizar el contacto",
                                                                     SweetAlertMessageType.success);
                        TempData.Keep();
                    }
                    else
                    {
                        // Valida Errores si Javascript está deshabilitado
                        Web.Utils.Util.ValidateErrors(this);
                        ViewBag.pais = listaPaises(Convert.ToInt32(pProveedores.IDPais));
                    

                        ViewBag.estado = listaEstados(Convert.ToInt32(pProveedores.estado));

                        return View("Edit", pProveedores);
                    }
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
        #endregion
        #region CRUD Contactos
        public ActionResult GuardarContacto(ViewModelContactos oViewModelContacto)
        {
            try
            {
                Convert.ToInt64(oViewModelContacto.telefono);
            }
            catch (Exception)
            {
                ViewBag.mensaje = "El teléfono del contacto debe contener sólo números";
                ViewBag.BackgroundColor = "red";
                return PartialView("_CardContactos", CrudContactos.Instancia);

            }



            if (oViewModelContacto.telefono.Length !=8 )
            {
                ViewBag.mensaje = "El teléfono del contacto debe contener 8 dígitos";
                ViewBag.BackgroundColor = "red";
                return PartialView("_CardContactos", CrudContactos.Instancia);
            }
            if (ValidarCamposVaciosContactos(oViewModelContacto))
            {
                ViewBag.mensaje = "El contacto contiene datos sin llenar";
                ViewBag.BackgroundColor = "red";
                return PartialView("_CardContactos", CrudContactos.Instancia);
            }
            if (ValidarRepetidos(oViewModelContacto))
            {

                ViewBag.mensaje = "El contacto está repetido";
                ViewBag.BackgroundColor = "red";
                return PartialView("_CardContactos", CrudContactos.Instancia);
            }
            ViewBag.BackgroundColor = "white";

            CrudContactos.Instancia.GuardarContacto(oViewModelContacto);
           
            return PartialView("_CardContactos", CrudContactos.Instancia);

        }
        public ActionResult ObtenerContacto(int id)
        {
            ViewModelContactos model = CrudContactos.Instancia.ObtenerContacto(id);
            return PartialView("_Contacto",model);
        }
        public ActionResult RemoverContacto(int id)
        {
            CrudContactos.Instancia.RemoverContacto(id);


            return PartialView("_CardContactos", CrudContactos.Instancia);
        }
        public ActionResult ActualizarContacto(int id)
        {


            CrudContactos.Instancia.ActualizarContacto(id);



            return PartialView("_CardContactos", CrudContactos.Instancia);
        }
        #endregion
        #region Listados
        private SelectList listaPaises(int IDPais = 0)
        {
            //Lista de autores
            ServiceProveedores _ServiceProveedor = new ServiceProveedores();
            IEnumerable<PAIS> listaPaises = _ServiceProveedor.GetPaises();
           

            //crea un combo box, los objetos son: lista, valor del objeto , y objeto a mostrar, y seleccionado
            return new SelectList(listaPaises, "ID", "nombre", IDPais);
        }
        private IEnumerable<CONTACTO> listaContactos(int? id)
        {
            //Lista de autores
            ServiceProveedores _ServiceProveedor = new ServiceProveedores();
            IEnumerable<CONTACTO> selectedContactos = _ServiceProveedor.GetContactosByProveedorID(id);
            //Autor SelectAutor = listaAutores.Where(c => c.IdAutor == idAutor).FirstOrDefault();

            //crea un combo box, los objetos son: lista, valor del objeto , y objeto a mostrar, y seleccionado
            return selectedContactos;


        }

        public IEnumerable<SelectListItem> listaEstados(int seleccionado)
        {
            
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "Activo", Value = "1"},
                new SelectListItem{Text = "Inactivo", Value = "0"},
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
        #endregion
        #region Validaciones
        public bool ValidarCamposVaciosContactos(ViewModelContactos oViewModelContacto)
        {
            bool isNull = false;
          
                if (oViewModelContacto.nombre == null || oViewModelContacto.correo == null || oViewModelContacto.telefono  == null)
                {
                    isNull = true;
                }
            
            return isNull;
        }
        public bool ValidarRepetidos(ViewModelContactos oViewModelContacto)
        {
            bool isRepeat = false;
            foreach (var item in CrudContactos.Instancia.Items)
            {
                if (oViewModelContacto.nombre.Trim() == item.nombre.Trim() && oViewModelContacto.correo.Trim() == item.correo.Trim() && oViewModelContacto.telefono.Trim() == item.telefono.Trim())
                {
                    isRepeat = true;
                }
            }


            return isRepeat;
        }
        public bool ValidarListaContactos(PROVEEDORES pProveedores)
        {
            bool isEmpty = false;
            if (CrudContactos.Instancia.Items.Count == 0)
            {
                isEmpty = true;
               

               

               
            }
            return isEmpty;
        }
        #endregion
        #region utilitarios
        public ActionResult ModalNuevo()
        {
            ViewModelContactos model = new ViewModelContactos();

            return PartialView("_Contacto", model);
        }
        #endregion



       
    }
}
