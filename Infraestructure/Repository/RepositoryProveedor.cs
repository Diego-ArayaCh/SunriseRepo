using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
  public  class RepositoryProveedor
    {
        public IEnumerable<PROVEEDORES> GetProveedores()
        {
            IEnumerable<PROVEEDORES> lista = null;
            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
               
                lista = ctx.PROVEEDORES.ToList();

            }
            return lista;
        }

        public PROVEEDORES GetProveedorByID(int pID)
        {
            PROVEEDORES oProveedor = null;
            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oProveedor = ctx.PROVEEDORES.Where(p => p.ID == pID).
                    Include(pr => pr.PRODUCTOS).
                    Include(c => c.CONTACTO).FirstOrDefault();
            }
            return oProveedor;



        }
    }
}
