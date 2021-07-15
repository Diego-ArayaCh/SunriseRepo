using Infraestructure.Models;
using Infraestructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
   public class ServiceInformes
    {
        public IEnumerable<HISTORICO> GetEntradas()
        {
            RepositoryInforme repository = new RepositoryInforme();
            return repository.GetEntradas();
        }
        public IEnumerable<HISTORICO> GetEntradas(DateTime from, DateTime to)
        {
            RepositoryInforme repository = new RepositoryInforme();
            return repository.GetEntradas(from, to);
        }

        public IEnumerable<HISTORICO> GetSalidas()
        {
            RepositoryInforme repository = new RepositoryInforme();
            return repository.GetSalidas();
        }
        public IEnumerable<HISTORICO> GetSalidas(DateTime from, DateTime to)
        {
            RepositoryInforme repository = new RepositoryInforme();
            return repository.GetSalidas(from, to);
        }
        public HISTORICO GetInformeById(int id)
        {
            RepositoryInforme repository = new RepositoryInforme();
            return repository.GetInformeByID(id);
        }

        public IEnumerable<PRODUCTOS> GetProductos_TOP3()
        {
            RepositoryInforme repository = new RepositoryInforme();
            return repository.GetProductos_TOP3();
        }
        public PRODUCTOS GetProductoByID(int idProducto)
        {
            RepositoryInforme repository = new RepositoryInforme();
            return repository.GetProductoByID(idProducto);
        }
    }
}
