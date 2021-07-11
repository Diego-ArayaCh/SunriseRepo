using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ViewModel
{
    public class ViewModelMovimiento
    {
        public MOVIMIENTO mov { get; set; }

        public List<PRODUCTOS> prodList { get; set; }

        public List<PRODUCTOS> prodListDetalle=new List<PRODUCTOS>();

        public USUARIO usuario { get; set; }

        public List<SUCURSAL> sucursalEntrada { get; set; }
        public List<SUCURSAL> sucursalSalir { get; set; }

        public HISTORICO historico { get; set; }

        public List<HistDetalleEntradaSalida> historicoDetalle { get; set; }

        public ViewModelMovimiento()
        {
        }
    }
}