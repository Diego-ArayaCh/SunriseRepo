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
                //lista = ctx.Libro.Include("PRODUCTOS").ToList();
                lista = ctx.HISTORICO.
                    Include("MOVIMIENTO").
                    Include("USUARIO").
                    Include("HistDetalleEntradaSalida").
                    Include("HistDetalleEntradaSalida.SUCURSAL").
                      Where(p => p.tipoMov == 1 || p.tipoMov == 3 ).
                                ToList();
                    

            }
            return lista;
        }
        public IEnumerable<HISTORICO> GetSalidas()
        {
            IEnumerable<HISTORICO> lista = null;
            using (MyContext ctx = new MyContext())
            {


                ctx.Configuration.LazyLoadingEnabled = false;
                //lista = ctx.Libro.Include("PRODUCTOS").ToList();
                lista = ctx.HISTORICO.Include("HistDetalleEntradaSalida").
                    Include("HistDetalleEntradaSalida.SUCURSAL1").
                    Include("MOVIMIENTO").
                      Where(p => p.tipoMov == 2 || p.tipoMov == 3).
                                ToList();


            }
            return lista;
        }
    }
}
