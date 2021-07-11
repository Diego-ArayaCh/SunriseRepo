using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
   public class ServiceMovimiento
    {
        public IEnumerable<PRODUCTOS> GetProductosActivoXProveedor(int idProveedor)
        {
            RepositoryMovimiento repository = new RepositoryMovimiento();
            return repository.GetProductosActivoXProveedor(idProveedor);
            //RepositoryProducto repository = new RepositoryProducto();
            //return repository.GetProductos();
        }



    }
}
