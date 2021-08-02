using ApplicationCore.Services;
using Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Security;
using Web.Util;

namespace Web.Controllers
{
    public class LogInController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(USUARIO usuario)
        {
            ServiceUsuario _ServiceUsuario = new ServiceUsuario();
            USUARIO oUsuario = null;
            try
            {
                if (ModelState.IsValid == false)
                {
                    oUsuario = _ServiceUsuario.GetUsuario(usuario.correo, usuario.contrasenha);

                    if (oUsuario != null)
                    {
                        if (oUsuario.estado == 1)
                        {
                            //Se crea variable USER en la session, para validar permisos y demas
                            Session["User"] = oUsuario;

                            Log.Info($"Accede {oUsuario.nombre} {oUsuario.apellidos} con el rol {oUsuario.ROL.ID}-{oUsuario.ROL.descripcion}");
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            Log.Warn($"{usuario.correo} se intentó conectar  y falló");
                            ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Login", "Error al autenticarse", SweetAlertMessageType.warning);
                        }
                        
                    }
                    else
                    {
                        Log.Warn($"{usuario.correo} se intentó conectar  y falló");
                        ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Login", "Error al autenticarse", SweetAlertMessageType.warning);

                    }
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData.Keep();
                return RedirectToAction("Default", "Error");
            }
        }

        public ActionResult Logout()
        {
            try
            {
                Log.Info("Usuario desconectado!");
                Session["User"] = null;
                return RedirectToAction("Index", "LogIn");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData["Redirect"] = "LogIn";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [CustomAuthorize((int)Roles.Administrador, (int)Roles.Encargado)]
        public ActionResult VerPerfil(int? id)
        {
            ServiceUsuario _ServiceUsuario = new ServiceUsuario();
            USUARIO oUsuario = null;
            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("IndexAdmin");
                }
                oUsuario = _ServiceUsuario.GetUsuarioByID(Convert.ToInt32(id));

                if (oUsuario == null)
                {
                    return RedirectToAction("Index", "Home");
                    //preguntar si vamos usar la pagina "error"
                }

                ViewBag.titulo = "Ver Perfil";
                return View(oUsuario);

            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult UnAuthorized()
        {
            try
            {
                ViewBag.Message = "No autorizado";

                if (Session["User"] != null)
                {
                    USUARIO oUsuario = Session["User"] as USUARIO;
                    Log.Warn($"El usuario {oUsuario.nombre} {oUsuario.apellidos} con el rol {oUsuario.ROL.ID}-{oUsuario.ROL.descripcion}, intentó acceder una página sin permisos  ");
                }

                return View();
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData["Redirect"] = "LogIn";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [HttpPost]
        public ActionResult Registrar(USUARIO user, string Contrasenia2)
        {
            USUARIO rUsuario = user;
            try
            {
                Convert.ToInt32(user.cedula);
            }
            catch (Exception e)
            {

                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Registro fallido", "Verifiqué que el campo de cédula contenga sólo números ", SweetAlertMessageType.warning);
                return View("Index");
            }
            try
            {
                Convert.ToInt32(user.telefono);
            }
            catch (Exception e)
            {

                ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Registro fallido", "Verifiqué que el campo de teléfono contenga sólo números ", SweetAlertMessageType.warning);
                return View("Index");
            }
            try
            {
                if (rUsuario.contrasenha == Contrasenia2)
                {
                    new ServiceUsuario().Save(rUsuario);
                    ModelState.Clear();
                    ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Registro exitoso", "Su usuario se ha registrado exitosamente \n Espere aprobación de un Administrador", SweetAlertMessageType.success);
                }
                else
                {
                    ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Registro Fallido", "Contraseña errónea, por favor revise", SweetAlertMessageType.warning);
                }

            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                // Pasar el Error a la página que lo muestra
                TempData["Message"] = ex.Message;
                TempData.Keep();
                return RedirectToAction("Default", "Error");
            }

            return View("Index");
        }
        [CustomAuthorize((int)Roles.Administrador)]
        //Mantenimiento de aprobaciones
        public ActionResult EditPermisos()
        {
            return View(new ServiceUsuario().GetUsuariosEncargados());
        }

        public ActionResult Habilitar(int? id)
        {
            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                USUARIO user = new ServiceUsuario().GetUsuarioByID(id.Value);
                if (user == null)
                {
                    return RedirectToAction("Index", "Home"); //preguntar si vamos usar la pagina "error"
                }

                user.estado = 1;
                new ServiceUsuario().Save(user);
                ViewBag.titulo = "Mantenimiento de Permisos";


            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("Index", "Home");
            }

            return View("EditPermisos", new ServiceUsuario().GetUsuariosEncargados());
        }

        public ActionResult Deshabilitar(int? id)
        {
            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                USUARIO user = new ServiceUsuario().GetUsuarioByID(id.Value);
                if (user == null)
                {
                    return RedirectToAction("Index", "Home"); //preguntar si vamos usar la pagina "error"
                }

                user.estado = 2;
                new ServiceUsuario().Save(user);
                ViewBag.titulo = "Mantenimiento de Permisos";


            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                return RedirectToAction("Index", "Home");
            }

            return View("EditPermisos", new ServiceUsuario().GetUsuariosEncargados());
        }

    }
}
