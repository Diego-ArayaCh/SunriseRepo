using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Util;
namespace Web.ViewModel
{
    public class CrudContactos
    {
        public List<ViewModelContactos> Items { get; private set; }
        public int contador = 0;
        //Implementación Singleton

        // Las propiedades de solo lectura solo se pueden establecer en la inicialización o en un constructor
       public static readonly CrudContactos Instancia;

        // Se llama al constructor estático tan pronto como la clase se carga en la memoria
     static CrudContactos()
        {
            // Si el carrito no está en la sesión, cree uno y guarde los items.
            if (HttpContext.Current.Session["CrudContactos"] == null)
            {
                Instancia = new CrudContactos();
                Instancia.Items = new List<ViewModelContactos>();
                
                HttpContext.Current.Session["CrudContactos"] = Instancia;
            }
            else
            {
                // De lo contrario, obténgalo de la sesión.
                Instancia = (CrudContactos)HttpContext.Current.Session["CrudContactos"];
            }
        }

        // Un constructor protegido asegura que un objeto no se puede crear desde el exterior
        protected CrudContactos() { }

        /**
         * AgregarItem (): agrega un artículo a la compra
         */
        public void ActualizarContacto(int id)
        {
            
         

        }
        public ViewModelContactos ObtenerContacto(int id)
        {
            ViewModelContactos oViewModelContactos = new ViewModelContactos() ;
            foreach (var item in Items)
            {
                if (item.IDProvisional == id)
                {
                    oViewModelContactos = item;
                }
            }
            return oViewModelContactos;
        }
        public void GuardarContacto(ViewModelContactos pContacto)
        {
            bool isExist = false;
           
            foreach (var item in Items.ToList())
            {
                if (item.IDProvisional == pContacto.IDProvisional)
                {
                    isExist = true;
                    pContacto.ID = item.ID;
                    pContacto.IDProv = item.IDProv;
                    Items.Remove(item);
                    Items.Add(pContacto);
                }
            }
            if (!isExist)
            {
                contador++;
                pContacto.IDProvisional = contador;

                Items.Add(pContacto);
            }

           

        }
        public void RemoverContacto(int id)
        {

          
                foreach (var item in Items.ToList())
                {
                    if (item.IDProvisional == id)
                    {
                        Items.Remove(item);
                       

                    }
                }
            }
        }
    }
