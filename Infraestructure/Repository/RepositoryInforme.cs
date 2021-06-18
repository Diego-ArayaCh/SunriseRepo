using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
  public  class RepositoryInforme
    {
        public IEnumerable<HISTORICO> GetEntradas()
        {
            IEnumerable<HISTORICO> lista = null;
            using (MyContext ctx = new MyContext())
            {
               

                ctx.Configuration.LazyLoadingEnabled = false;

                // lista = ctx.Libro.Include("PRODUCTOS").ToList();
                lista = ctx.HISTORICO.
                    Include("MOVIMIENTO").
                    Include("USUARIO").
                    Include("HistDetalleEntradaSalida").
                    Include("HistDetalleEntradaSalida.SUCURSAL").
                      Where(p => p.tipoMov == 1 || p.tipoMov == 3).
                                ToList();

                //lista = ctx.HistDetalleEntradaSalida.
                //   Include("PRODUCTOS").
                //   Include("SUCURSAL").
                //   Include("HISTORICO").
                //   Include("HISTORICO.MOVIMIENTO").
                //   Include("HISTORICO.USUARIO").
                //     Where(p => p.IDSucursalEntra != null).
                //               ToList();
            }
            return lista;
        }
        public IEnumerable<HistDetalleEntradaSalida> GetSalidas()
        {
            IEnumerable<HistDetalleEntradaSalida> lista = null;
            using (MyContext ctx = new MyContext())
            {


                ctx.Configuration.LazyLoadingEnabled = false;
                 lista = ctx.HistDetalleEntradaSalida.
                   Include("PRODUCTOS").
                   Include("SUCURSAL1").
                   Include("HISTORICO").
                   Include("HISTORICO.MOVIMIENTO").
                   Include("HISTORICO.USUARIO").
                     Where(p => p.IDSucursalEntra == null).
                               ToList();


            }
            return lista;
        }
    }
}
