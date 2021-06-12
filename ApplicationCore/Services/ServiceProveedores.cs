using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
  public  class ServiceProveedores
    {
        public IEnumerable<PROVEEDORES> GetProveedores()
        {
            RepositoryProveedor repository = new RepositoryProveedor();
            return repository.GetProveedores();
        }

        public PRODUCTOS GetProductoByID(int pID)
        {
            RepositoryProducto repository = new RepositoryProducto();
            return repository.GetProductoByID(pID);



        }
    }
}
