using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceProveedores
    {
        public IEnumerable<PAIS> GetPaises()
        {
            RepositoryProveedor repository = new RepositoryProveedor();
            return repository.GetPaises();
        }
        public IEnumerable<CONTACTO> GetContactosByProveedorID(int? id)
        {
            RepositoryProveedor repository = new RepositoryProveedor();
            return repository.GetContactosByProveedorID(id);
        }
        public IEnumerable<PROVEEDORES> GetProveedores()
        {
            RepositoryProveedor repository = new RepositoryProveedor();
            return repository.GetProveedores();
        }
        public IEnumerable<string> GetProveedoresNombre()
        {
            RepositoryProveedor repository = new RepositoryProveedor();
            return repository.GetProveedores().Select(x => x.nombre);
        }
        public IEnumerable<PROVEEDORES> GetProductosxNombre(string pFiltro)
        {
            RepositoryProveedor repository = new RepositoryProveedor();
            return repository.GetProveedorByNombre(pFiltro);
        }
        public PROVEEDORES GetProveedorByID(int pID)
        {
            RepositoryProveedor repository = new RepositoryProveedor();
            return repository.GetProveedorByID(pID);
        }
        public PROVEEDORES Save(PROVEEDORES oProveedor, List<CONTACTO> contactos)
        {
            RepositoryProveedor repository = new RepositoryProveedor();
            return repository.Save(oProveedor,contactos);
        }
        public CONTACTO GetContactoByID(int id)
        {
            RepositoryProveedor repository = new RepositoryProveedor();
            return repository.GetContactoByID(id);
        }
        
        }
}
