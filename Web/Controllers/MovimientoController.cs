using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class MovimientoController : Controller
    {

        // GET: Movimiento
        public ActionResult MovimientoEntrada()
        {
            return View();
        }

        public ActionResult MovimientoSalida()
        {
            return View();
        }

        public ActionResult MovimientoTransferencia()
        {
            return View();
        }



    }
}
