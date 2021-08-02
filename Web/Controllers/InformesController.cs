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
using System.Globalization;
using Web.Security;

namespace Web.Controllers
{
    public class InformesController : Controller
    {
        // GET: Informe
        /* public ActionResult InformeEntrada(int? page)
         {
             IEnumerable<HISTORICO> lista = null;
             try
             {
                  ServiceInformes _ServiceInformes = new ServiceInformes();

                 lista = _ServiceInformes.GetEntradas();
             }
             catch (Exception ex)
             {
                 // Salvar el error en un archivo 
                 Log.Error(ex, MethodBase.GetCurrentMethod());
                 return RedirectToAction("Index", "Home");
             }

             ViewBag.titulo = "Lista Entradas";

             int pageSize = 4;
             int pageNumber = page ?? 1;

             return View(lista.ToPagedList(pageNumber,pageSize));


         }*/

        //[HttpPost]
        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult InformeEntrada(String from, String to, int? page)
        {
         
                IEnumerable<HISTORICO> lista = null;
            List<HISTORICO> model = new List<HISTORICO>();
            try
                {
                    
                    ServiceInformes _ServiceInformes = new ServiceInformes();
                    if (String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to))
                    {
                    ViewBag.From = "";
                    ViewBag.To = "";
                        lista = _ServiceInformes.GetEntradas();
                    }
                    else
                    {
                    IEnumerable<HISTORICO> temp = _ServiceInformes.GetEntradas();
                    DateTime inicio, fin;
                        inicio = DateTime.ParseExact(from,"MM/dd/yyyy", CultureInfo.InvariantCulture);
                        fin= DateTime.ParseExact(to, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    List<HISTORICO> alma = new List<HISTORICO>();
                    foreach(HISTORICO hist in temp)
                    {
                        lista = new List<HISTORICO>();
                        //String[] cadena = hist.fechaHora.Split(' ');
                        //String fec = cadena[0] + "/" + cadena[1] + "/" + cadena[2];
                        DateTime fecha = DateTime.ParseExact(hist.fechaHora, "dd/MM/yyyy hh:mmtt", CultureInfo.InvariantCulture);
                        if (DateTime.Compare(fecha, inicio) >= 0 && DateTime.Compare(fecha, fin)<=0){
                           alma.Add(hist);
                        }
                    }
                    lista = alma;
                    ViewBag.From = from;
                    ViewBag.To = to;
                    // lista = _ServiceInformes.GetEntradas(inicio, fin);
                }
                USUARIO user = ((USUARIO)Session["User"]);
               
                foreach (var item in lista)
                {
                    

                    if (user.IDRol == 2)
                    {
                        if (item.IDUsuario==user.ID)
                        {
                            model.Add(item);
                        }
                    }
                    else
                    {
                        model = lista.ToList();
                    }
                }
                }
                catch (Exception ex)
                {
                    // Salvar el error en un archivo 
                    Log.Error(ex, MethodBase.GetCurrentMethod());
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.titulo = "Lista Entradas";

                int pageSize = 4;
                int pageNumber = page ?? 1;

                return View("InformeEntrada", model.ToPagedList(pageNumber, pageSize));
               
           

        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        // GET: Informe/Details/5
        public ActionResult Details(int? id)
        {
            ServiceInformes _ServiceInforme = new ServiceInformes();
            HISTORICO informe = null;
            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                informe = _ServiceInforme.GetInformeById(id.Value);
                if (informe == null)
                {
                    return RedirectToAction("Index", "Home"); //preguntar si vamos usar la pagina "error"
                }

                ViewBag.titulo = "Detalle Informe";
                return View(informe);

            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("Index", "Home");
            }
        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult InformeSalida(String from, String to, int? page)
        {
            IEnumerable<HISTORICO> lista = null;
            List<HISTORICO> model = new List<HISTORICO>();
            try
            {
                ServiceInformes _ServiceInformes = new ServiceInformes();

                
                if (String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to))
                {
                    ViewBag.From = "";
                    ViewBag.To = "";
                    lista = _ServiceInformes.GetSalidas();
                }
                else
                {
                    IEnumerable<HISTORICO> temp = _ServiceInformes.GetSalidas();
                    DateTime inicio, fin;
                    inicio = DateTime.ParseExact(from, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    fin = DateTime.ParseExact(to, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    List<HISTORICO> alma = new List<HISTORICO>();
                    foreach (HISTORICO hist in temp)
                    {
                        lista = new List<HISTORICO>();
                        //String[] cadena = hist.fechaHora.Split(' ');
                        //String fec = cadena[0] + "/" + cadena[1] + "/" + cadena[2];
                        DateTime fecha = DateTime.ParseExact(hist.fechaHora, "dd/MM/yyyy hh:mmtt", CultureInfo.InvariantCulture);
                        if (DateTime.Compare(fecha, inicio) >= 0 && DateTime.Compare(fecha, fin) <= 0)
                        {
                            alma.Add(hist);
                        }
                    }
                    ViewBag.From = from;
                    ViewBag.To = to;
                    lista = alma;
                    // lista = _ServiceInformes.GetEntradas(inicio, fin);
                }
                USUARIO user = ((USUARIO)Session["User"]);

                foreach (var item in lista)
                {


                    if (user.IDRol == 2)
                    {
                        if (item.IDUsuario == user.ID)
                        {
                            model.Add(item);
                        }
                    }
                    else
                    {
                        model = lista.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("Index", "Home");
            }

            ViewBag.titulo = "Lista Salidas";
            int pageSize = 4;
            int pageNumber = page ?? 1;

            return View(model.ToPagedList(pageNumber, pageSize));
        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult InformeProductos()
        {
            IEnumerable<PRODUCTOS> lista = null;
            try
            {
                ServiceInformes _ServiceInformes = new ServiceInformes();
                lista = _ServiceInformes.GetProductos_TOP3();
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("Index", "Home");
            }

            ViewBag.titulo = "Informe Productos";
            return View(lista);
        }
        
         [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult DetailsProducto(int? id)
        {
            ServiceInformes _ServiceInforme = new ServiceInformes();
            PRODUCTOS informe = null;
            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                informe = _ServiceInforme.GetProductoByID(id.Value);
                if (informe == null)
                {
                    return RedirectToAction("Index", "Home"); //preguntar si vamos usar la pagina "error"
                }

                ViewBag.titulo = "Detalle Informe";
                return View(informe);

            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("Index", "Home");
            }
        }




    }
}
