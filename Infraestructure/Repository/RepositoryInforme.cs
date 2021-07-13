using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
        public IEnumerable<HISTORICO> GetEntradas(DateTime from, DateTime to)
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
                      Where(p => (DateTime.Compare( Convert.ToDateTime(p.fechaHora),from)>0 && DateTime.Compare(Convert.ToDateTime(p.fechaHora),to)<0)&&(p.tipoMov == 1 || p.tipoMov == 3) ).
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
        public HISTORICO GetInformeByID(int pID)
        {
            HISTORICO oHistorico = null;
            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;

                oHistorico = ctx.HISTORICO.
                    Include("MOVIMIENTO").
                    Include("USUARIO").
                    Include("USUARIO.ROL").
                    Include("HistDetalleEntradaSalida").
                    Include("HistDetalleEntradaSalida.PRODUCTOS").
                    Include("HistDetalleEntradaSalida.PRODUCTOS.CATEGORIA").
                    Include("HistDetalleEntradaSalida.PROVEEDORES").
                    Include("HistDetalleEntradaSalida.SUCURSAL").
                    Include("HistDetalleEntradaSalida.SUCURSAL1").
                    Where(p => p.ID == pID ).FirstOrDefault<HISTORICO>();

            }
            return oHistorico;

        }


        public IEnumerable<HISTORICO> GetSalidas()
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
                    Include("HistDetalleEntradaSalida.SUCURSAL1").
                      Where(p => p.tipoMov == 2 || p.tipoMov == 3).
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
        public IEnumerable<HISTORICO> GetSalidas(DateTime from, DateTime to)
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
                    Include("HistDetalleEntradaSalida.SUCURSAL1").
                      Where(p => (Convert.ToDateTime(p.fechaHora) >= from && Convert.ToDateTime(p.fechaHora) <= to) && (p.tipoMov == 2 || p.tipoMov == 3)).
                                ToList(); ;

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




        public IEnumerable<PRODUCTOS> GetProductos_TOP3()
        {
            try
            {
                IEnumerable<PRODUCTOS> lista = null;
                IEnumerable<PRODUCTOS> listaAUX = new List<PRODUCTOS>();

                using (MyContext ctx = new MyContext())
                {

                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.PRODUCTOS.Include("CATEGORIA").
                            Include("PROVEEDORES").
                            Include("PROVEEDORES.PAIS").
                            Include("ProdSuc").
                            Include("ProdSuc.SUCURSAL").
                            Include("HistDetalleEntradaSalida").ToList();

                    foreach(var item in lista)
                    {
                        int sum=0;
                        foreach (var item2 in item.HistDetalleEntradaSalida)
                        {
                             sum=+ (int) item2.cantidad; //Suma la cantidad de objetos de cada salida
                        }
                        item.cantMin = sum; //Va ser utilizada para almacenar el total de objetos movidos
                    }

                    int total = lista.Count(); // 30
                    listaAUX.Append(lista.OrderBy(i => i.cantMin).ElementAt(total - 0));//ultimo
                    listaAUX.Append(lista.OrderBy(i => i.cantMin).ElementAt(total - 1));//antepenultimo
                    listaAUX.Append(lista.OrderBy(i => i.cantMin).ElementAt(total - 2));//penultimo

                }
                return listaAUX;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }


    }
}
