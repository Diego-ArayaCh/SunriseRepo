using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceProductos
    {
        public IEnumerable<PRODUCTOS> GetProductos()
        {
            RepositoryProducto repository = new RepositoryProducto();
            return repository.GetProductos();
        }

        public IEnumerable<string> GetProductoNombres()
        {
            RepositoryProducto repository = new RepositoryProducto();
            return repository.GetProductos().Select(x => x.nombre);
        }

        public IEnumerable<PRODUCTOS> GetProductosxNombre( string pFiltro)
        {
            RepositoryProducto repository = new RepositoryProducto();
            return repository.GetProductoByNombre(pFiltro);
        }

        public IEnumerable<CATEGORIA> GetCategorias()
        {
            RepositoryProducto repository = new RepositoryProducto();
            return repository.GetCategorias();
        }

        public IEnumerable<SUCURSAL> GetSucursales()
        {
            RepositoryProducto repository = new RepositoryProducto();
            return repository.GetSucursales();
        }

        public IEnumerable<PROVEEDORES> GetProveedores()
        {
            RepositoryProducto repository = new RepositoryProducto();
            return repository.GetProveedores();
        }

        public PRODUCTOS GetProductoByID(int pID)
        {
            RepositoryProducto repository = new RepositoryProducto();
            return repository.GetProductoByID(pID);
        }

        public PRODUCTOS Save(PRODUCTOS oProducto,  string[] selectedSucursales, string[] selectedProveedores)
        {
            RepositoryProducto repository = new RepositoryProducto();
            return repository.Save(oProducto, selectedSucursales, selectedProveedores);
        }

    }
}
