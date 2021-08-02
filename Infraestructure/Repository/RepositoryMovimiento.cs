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
    public class RepositoryMovimiento
    {

        public IEnumerable<PRODUCTOS> GetProductosActivoXProveedor(int idProveedor)
        {
            try
            {
                IEnumerable<PRODUCTOS> lista = null;
                ICollection<PRODUCTOS> lista_ProdFiltrados = new List<PRODUCTOS>();
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.PRODUCTOS.Include("CATEGORIA").Include("PROVEEDORES").ToList().
                        FindAll(l => l.estado == 1);

                    foreach (var item in lista)
                    {
                        foreach (var prod in item.PROVEEDORES)
                        {
                            if (prod.ID == idProveedor)
                            {
                                lista_ProdFiltrados.Add(item);
                            }
                        }
                    }



                }
                return lista_ProdFiltrados;
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
        
        public IEnumerable<PRODUCTOS> GetProductosActivoXSucursal(int idSucursal)
        {
            try
            {
                IEnumerable<PRODUCTOS> lista = null;
                ICollection<PRODUCTOS> lista_ProdFiltrados = new List<PRODUCTOS>();
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.PRODUCTOS.
                            Include("CATEGORIA").
                            Include("PROVEEDORES").
                            Include("PROVEEDORES.PAIS").
                            Include("ProdSuc").
                            Include("ProdSuc.SUCURSAL").
                            ToList().
                        FindAll(l => l.estado == 1);

                    foreach (var item in lista)
                    {
                        foreach (var prod in item.ProdSuc)
                        {
                            if (prod.IDSucursal == idSucursal)
                            {
                                if (prod.cant >= 1)
                                {
                                    lista_ProdFiltrados.Add(item);
                                }
                            }
                        }
                    }
                }
                return lista_ProdFiltrados;
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


        public IEnumerable<SUCURSAL> GetSucursales()
        {
            try
            {
                IEnumerable<SUCURSAL> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    lista = ctx.SUCURSAL.ToList();
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
        public HISTORICO GuardarMovimiento(HISTORICO mov)
        {
            HISTORICO histMov = null;
            histMov = mov;
            int retorno;
            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;

                if (histMov != null)
                {
                    try
                    {
                        using (var transaccion = ctx.Database.BeginTransaction())
                        {//Insertar nuevo historico con el detalle?
                            ctx.HISTORICO.Add(histMov);
                            retorno = ctx.SaveChanges();

                            //Actualizacion de cantidades

                            foreach (HistDetalleEntradaSalida detalle in histMov.HistDetalleEntradaSalida)
                            {
                                switch (histMov.tipoMov)
                                {
                                    case 1:
                                        ProdSuc ps = ctx.ProdSuc.Where(p => p.IDProducto == detalle.IDProducto && p.IDSucursal == detalle.IDSucursalEntra).FirstOrDefault();
                                        if (ps==null)
                                        {
                                            ps = new ProdSuc();
                                            ps.IDProducto = detalle.IDProducto;
                                            ps.IDSucursal = detalle.IDSucursalEntra.Value;
                                            ps.cant = detalle.cantidad;
                                            ctx.ProdSuc.Add(ps);
                                            ctx.SaveChanges();
                                        }
                                        else
                                        {
                                            ps.cant += detalle.cantidad;
                                            ctx.Entry(ps).State = EntityState.Modified;
                                        }
                                        
                                        PRODUCTOS prod = ctx.PRODUCTOS.
                                        Where(p => p.ID == detalle.IDProducto).
                                        FirstOrDefault<PRODUCTOS>();
                                        
                                        prod.stock += detalle.cantidad;
                                        ctx.Entry(prod).State = EntityState.Modified;
                                        break;
                                    case 2:
                                        PRODUCTOS prod1 = ctx.PRODUCTOS.
                                        Where(p => p.ID == detalle.IDProducto).
                                        FirstOrDefault<PRODUCTOS>();
                                        prod1.stock -= detalle.cantidad;
                                        ctx.Entry(prod1).State = EntityState.Modified;
                                        ProdSuc ps1 = ctx.ProdSuc.Where(p => p.IDProducto == detalle.IDProducto && p.IDSucursal == detalle.IDSucursalSale.Value).First();
                                        ps1.cant -= detalle.cantidad;
                                        ctx.Entry(ps1).State = EntityState.Modified;
                                        break;
                                    //case 3:
                                    //    ProdSuc psEntra = ctx.ProdSuc.Where(p => p.IDProducto == detalle.IDProducto && p.IDSucursal == detalle.IDSucursalEntra.Value).First();
                                    //    psEntra.cant += detalle.cantidad;
                                    //    ProdSuc psSalida = ctx.ProdSuc.Where(p => p.IDProducto == detalle.IDProducto && p.IDSucursal == detalle.IDSucursalSale.Value).First();
                                    //    psSalida.cant -= detalle.cantidad;
                                    //    ctx.Entry(psEntra).State = EntityState.Modified;
                                    //    ctx.Entry(psSalida).State = EntityState.Modified;
                                    //    break;

                                }
                                ctx.SaveChanges();
                            }

                            
                            transaccion.Commit();
                        }




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


                

                return histMov;
            }

        }
        public ProdSuc GetProdSucByBothID(int idProducto, int idSucursal)
        {
            ProdSuc ps=null;
            using (MyContext ctx=new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                ps = ctx.ProdSuc.Where(p => p.IDProducto == idProducto && p.IDSucursal == idSucursal).First();
            }
            return ps;
            
        }

    }
    
}
