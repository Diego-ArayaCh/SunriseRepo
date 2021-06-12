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
    public class InformesController : Controller
    {
        // GET: Informe
        public ActionResult InformeEntrada()
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
            return View(lista);

           
        }

        // GET: Informe/Details/5
        public ActionResult InformeSalida()
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
            return View(lista);

        }

        // GET: Informe/Create

    }
}
