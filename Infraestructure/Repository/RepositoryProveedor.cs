using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RepositoryProveedor
    {
        public IEnumerable<PAIS> GetPaises()
        {
            try
            {
                IEnumerable<PAIS> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.PAIS.ToList<PAIS>();
                }
                return lista;

            }

            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }
        public IEnumerable<PROVEEDORES> GetProveedorByNombre(string pFiltro)
        {
            try
            {
                IEnumerable<PROVEEDORES> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.PROVEEDORES.ToList().
                         FindAll(l => l.nombre.ToLower().Contains(pFiltro.ToLower()));
                }
                return lista;
            }

            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<PROVEEDORES> GetProveedores()
        {
            try
            {
                IEnumerable<PROVEEDORES> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.PROVEEDORES.ToList();
                }
                return lista;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);

            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;


            }

        }
        public void DeleteNullsContactos()
        {
            using (SunriseBDEntities1 ctx = new SunriseBDEntities1())
            {
                ctx.Database.ExecuteSqlCommand("delete from CONTACTOS where idprov = {0}");
            }
        }
        public PROVEEDORES GetProveedorByID(int pID)
        {
            try
            {
                PROVEEDORES oProveedor = null;

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oProveedor = ctx.PROVEEDORES.Where(p => p.ID == pID).
                        Include("PRODUCTOS").
                        Include("PRODUCTOS.CATEGORIA").
                        Include("CONTACTO").
                        Include("PAIS").
                        FirstOrDefault();
                }
                return oProveedor;

            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);

            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }



        }
  
        public CONTACTO GetContactoByID(int id)
        {
            CONTACTO contacto = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    contacto = ctx.CONTACTO.Find(id);
                }

                return contacto;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }
        public IEnumerable<CONTACTO> GetContactosByProveedorID(int? id)
        {
           
            try
            {

                IEnumerable<CONTACTO> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    if (id != null)
                    {
                        lista = ctx.CONTACTO.Where(x => x.IDProv == id).ToList();
                    }
                    else
                    {
                        lista = ctx.CONTACTO.ToList();
                    }
                  
                   
                }
                return lista;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }
       
        public PROVEEDORES Save(PROVEEDORES pProveedor,List<CONTACTO> contactos)
        {
         
          
            
            int retorno = 0;
            PROVEEDORES oProveedor = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oProveedor = GetProveedorByID(pProveedor.ID);

                    if (oProveedor == null)//si es null es nuevo y sino es edit
                    {
                        pProveedor.estado = 0;
                        pProveedor.CONTACTO = new List<CONTACTO>();
                        using (var transaccion = ctx.Database.BeginTransaction())
                        {
                            ctx.PROVEEDORES.Add(pProveedor);
                            retorno = ctx.SaveChanges();

                            //Insertar



                            foreach (var contacto in contactos)
                            {

                                //ctx.CONTACTO.Attach(contacto); //sin esto, EF intentará crear una categoría
                                pProveedor.CONTACTO.Add(contacto);// asociar a la categoría existente con el libro

                                retorno = ctx.SaveChanges();

                            }



                            // Commit 
                            transaccion.Commit();
                        }

                    }
                    else
                    {

                        //Registradas: 1,2,3
                        //Actualizar: 1,3,4

                        //Actualizar proveedor


                        ctx.PROVEEDORES.Add(pProveedor);
                        ctx.Entry(pProveedor).State = EntityState.Modified;
                        retorno = ctx.SaveChanges();

                        //Actualizar contactos
                        using (var transaccion = ctx.Database.BeginTransaction())
                        {

                            List<String> listContactosID = new List<String>();
                            foreach (var item in contactos)
                            {
                                listContactosID.Add(item.ID.ToString());
                            }

                            var listContactos = new HashSet<string>(listContactosID);
                            if (pProveedor.CONTACTO != null)

                                ctx.Entry(pProveedor).Collection(p => p.CONTACTO).Load();

                            var new_C_ForProveedor = ctx.CONTACTO
                             .Where(x => listContactos.Contains(x.ID.ToString())).ToList();
                            ICollection<CONTACTO> insertPS = new List<CONTACTO>();
                            foreach (CONTACTO itemContacto in contactos)
                            {

                                CONTACTO oContacto = new CONTACTO();
                                oContacto.ID = itemContacto.ID;
                                oContacto.IDProv = pProveedor.ID;
                                oContacto.nombre = itemContacto.nombre;
                                oContacto.telefono = itemContacto.telefono;
                                oContacto.correo = itemContacto.correo;
                                oContacto.estado = 1;
                                
                                insertPS.Add(oContacto);





                            }
                            pProveedor.CONTACTO = insertPS;
                            ctx.Entry(pProveedor).State = EntityState.Modified;

                           
                           
                            retorno = ctx.SaveChanges();
                            transaccion.Commit();
                        }
                    }
                    }
                using (MyContext context = new MyContext())
                {
                    List<CONTACTO> listaNull = context.CONTACTO.Where(c => c.IDProv == null).ToList();
                    foreach (var item in listaNull)
                    {
                        context.Entry(item).State = EntityState.Deleted;
                        retorno = context.SaveChanges();
                    }
                }

                if (retorno >= 0)
                        oProveedor = GetProveedorByID((int)pProveedor.ID);

                    return oProveedor;

                
                
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
        }
    }
}


