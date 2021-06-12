using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RepositoryProducto
    {
        public IEnumerable<PRODUCTOS> GetProductos()
        {
            IEnumerable<PRODUCTOS> lista = null;
            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                //lista = ctx.Libro.Include("PRODUCTOS").ToList();
                lista = ctx.PRODUCTOS.Include("CATEGORIA").ToList();

            }
            return lista;
        }

        public PRODUCTOS GetProductoByID(int pID)
        {
            PRODUCTOS oProducto = null;
            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;

                oProducto = ctx.PRODUCTOS.
                        Include("CATEGORIA").
                        Include("PROVEEDORES").
                        Include("PROVEEDORES.PAIS").
                        Include("ProdSuc").
                        Include("ProdSuc.SUCURSAL").
                            Where(p => p.ID == pID).
                                FirstOrDefault<PRODUCTOS>();
            }
            return oProducto;

        }

    }
}
