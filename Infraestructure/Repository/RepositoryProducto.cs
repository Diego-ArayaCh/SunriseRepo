using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                //lista = ctx.Libro.Include("Autor").ToList();
                lista = ctx.PRODUCTOS.ToList();

            }
            return lista;
        }

        public  PRODUCTOS GetProductoByID(int pID)
        {
            PRODUCTOS oProducto = null;
            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;

                var productoBuscado = from p in ctx.PRODUCTOS
                                      join ps in ctx.ProdSuc on p.ID equals ps.IDProducto
                                      join s in ctx.SUCURSAL on ps.IDSucursal equals s.ID
                                      where p.ID == pID
                                      select new PRODUCTOS
                                      {
                                          //variables a requerir de la consulta
                                          nombre = p.nombre,
                                          serial = p.serial
                                      };
                oProducto = productoBuscado.First();
                
                return oProducto;
               


                //    oProducto = ctx.PRODUCTOS.
                //Where(p => p.ID == pID).
                //Include(c => c.CATEGORIA).
                //Include(pr => pr.PROVEEDORES).
                //Include(u => u.ProdSuc).
                //FirstOrDefault();
            }
           


           
        }

    }
}
