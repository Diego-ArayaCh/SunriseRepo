using System;
using System.Collections.Generic;
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
            ViewBag.usuario = "Jose M. Figueres";

            return View();
        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult About()
        {
            ViewBag.Message = "Desarrolladores involucrados en la app";

            return View();
        }

    

    }
}