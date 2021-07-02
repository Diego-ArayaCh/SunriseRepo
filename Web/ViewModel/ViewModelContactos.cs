using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.ViewModel
{
    public class ViewModelContactos
    {
     

        public int IDProvisional { get; set; }
        public int ID { get; set; }
        public Nullable<int> IDProv { get; set; }
        public Nullable<int> estado { get; set; }

        public string nombre { get; set; }

        public string correo { get; set; }

        public string telefono { get; set; }
        public virtual CONTACTO contacto { get; set; }
        public virtual ViewModelContactos instancia()
        {
            return this;

        }
        public ViewModelContactos()
        {
           
        }
        public ViewModelContactos(int ID)
        {
            ServiceProveedores serviceProveedores = new ServiceProveedores();
            this.ID = ID;
            this.contacto = serviceProveedores.GetContactoByID(ID);
        }
    }
}