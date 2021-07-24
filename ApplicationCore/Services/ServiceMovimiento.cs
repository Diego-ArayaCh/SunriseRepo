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
        
        public IEnumerable<PRODUCTOS> GetProductosActivoXSucursal(int idSucursal)
        {
            RepositoryMovimiento repository = new RepositoryMovimiento();
            return repository.GetProductosActivoXSucursal(idSucursal);
        }

        public IEnumerable<SUCURSAL> GetSucursales()
        {
            RepositoryMovimiento repository = new RepositoryMovimiento();
            return repository.GetSucursales();
        }
        public HISTORICO GuardarMovimiento(HISTORICO hist)
        {
            return new RepositoryMovimiento().GuardarMovimiento(hist);
        }


    }
}
