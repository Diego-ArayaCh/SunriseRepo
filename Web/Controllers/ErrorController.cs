using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(Exception exception)
        {
            //Controlador
            String redirect = "Home";
            //Acción de controlador
            String redirectAction = "Index";

            HttpException httpException = exception as HttpException;
            //Obtener el código de estado HTTP
            int error = httpException != null ? httpException.GetHttpCode() : 0;
            switch (error)
            {
                case 400:
                    ViewBag.Title = "Solicitud incorrecta";
                    ViewBag.Description = "El servidor no pudo interpretar la solicitud dada una sintaxis inválida.";
                    break;

                case 401:
                    ViewBag.Title = "No autorizado";
                    ViewBag.Description = "Es necesario autenticar el usuario para obtener la respuesta solicitada";
                    break;
                case 403:
                    ViewBag.Title = "Acceso restringido";
                    ViewBag.Description = "El usuario no posee los permisos necesarios para el contenido";
                    break;
                default:
                    ViewBag.Title = error + " Error";
                    ViewBag.Description = exception.Message;
                    break;
            }
            ViewBag.redirect = redirect;
            ViewBag.redirectAction = redirectAction;

            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Default()
        {
            //Controlador
            String redirect = "Home";
            //Acción de controlador
            String redirectAction = "Index";
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Title = "Error";
                ViewBag.Description = TempData["Message"];
                if (TempData.ContainsKey("Redirect"))
                {
                    redirect = (String)TempData["Redirect"];
                }
                if (TempData.ContainsKey("Redirect-Action"))
                {
                    redirectAction = (String)TempData["Redirect-Action"];
                }
                ViewBag.redirect = redirect;
                ViewBag.redirectAction = redirectAction;
                return View("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

    }
}