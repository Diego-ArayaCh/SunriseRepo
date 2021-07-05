using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Security;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult Index()
        {
            IEnumerable<PRODUCTOS> productos = new ServiceProductos().GetProductos();
            IEnumerable<HISTORICO> entradas = new ServiceInformes().GetEntradas();
            IEnumerable<HISTORICO> salidas = new ServiceInformes().GetSalidas();


            int cantEntradas, cantSalidas;

            //cantEntradas = entradas.Count();
            //cantSalidas = salidas.Count();

            cantEntradas = 0;cantSalidas = 0;
            DateTime hora24 = DateTime.Now.Add(new TimeSpan(-24, 0, 0));
            foreach(HISTORICO ent in entradas)
            {
                if (DateTime.Compare(hora24,DateTime.ParseExact(ent.fechaHora, "dd/MM/yyyy hh:mmtt", CultureInfo.InvariantCulture))<0)
                {
                    cantEntradas++;
                }
            }
            foreach (HISTORICO ent in salidas)
            {
                if (DateTime.Compare(hora24, DateTime.ParseExact(ent.fechaHora, "dd/MM/yyyy hh:mmtt", CultureInfo.InvariantCulture)) < 0)
                {
                    cantSalidas++;
                }
            }

            ViewBag.CantSalidos = cantSalidas;
            ViewBag.CantEntradas =cantEntradas;
            ViewBag.CantProductos = productos.Count();
            List<PRODUCTOS> tempProd = new List<PRODUCTOS>();
            foreach(PRODUCTOS pr in productos)
            {
                int i = 0;
                foreach(ProdSuc ps in pr.ProdSuc)
                {
                    if (ps.cant < pr.cantMin) i++;
                }
                if (i > 0) tempProd.Add(pr);
            }
            productos = tempProd;

            ViewBag.usuario = "Jose M. Figueres";

            return View(productos);
        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult About()
        {
            ViewBag.Message = "Desarrolladores involucrados en la app";

            return View();
        }

    

    }
}