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

namespace Web.Controllers
{
    public class InformesController : Controller
    {
        // GET: Informe
        public ActionResult InformeEntrada(int? page)
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

           
        }


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

        public ActionResult InformeSalida(int? page)
        {
            IEnumerable<HISTORICO> lista = null;
            try
            {
                ServiceInformes _ServiceInformes = new ServiceInformes();

                lista = _ServiceInformes.GetSalidas();
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

            return View(lista.ToPagedList(pageNumber, pageSize));
        }

        // GET: Informe/Create

    }
}
