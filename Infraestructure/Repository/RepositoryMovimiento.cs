using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RepositoryMovimiento
    {

        public IEnumerable<PRODUCTOS> GetProductosActivoXProveedor(int idProveedor)
        {
            try
            {
                IEnumerable<PRODUCTOS> lista = null;
                ICollection<PRODUCTOS> lista_ProdFiltrados = new List<PRODUCTOS>();
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.PRODUCTOS.Include("CATEGORIA").Include("PROVEEDORES").ToList().
                        FindAll(l => l.estado == 1);

                    foreach(var item in lista)
                    {
                        foreach(var prod in item.PROVEEDORES)
                        {
                            if (prod.ID == idProveedor)
                            {
                                lista_ProdFiltrados.Add(item);
                            }
                        }
                    }



                }
                return lista_ProdFiltrados;
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

        public IEnumerable<SUCURSAL> GetSucursales()
        {
            try
            {
                IEnumerable<SUCURSAL> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx. SUCURSAL.ToList();
                }
                return lista;
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
