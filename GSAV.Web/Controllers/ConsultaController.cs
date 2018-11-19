using GSAV.Entity.Objects;
using GSAV.ServiceContracts.Interface;
using GSAV.Web.Models;
using GSAV.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GSAV.Web.Controllers
{
    [Authorize]
    public class ConsultaController : Controller
    {

        private readonly IBLSolicitud oIBLSolicitud;

        public ConsultaController(IBLSolicitud bLSolicitud)
        {
            oIBLSolicitud = bLSolicitud;
        }

        public ActionResult Index()
        {
            if (Session["Login-Info"] == null)
            {
                return this.RedirectToAction("Login", "Account");
            }

            if (this.Request.IsAuthenticated)
            {
                if (Session["Login-Info"] != null)
                {
                    var user = ((Entity.Util.ReturnObject<Usuario>)Session["Login-Info"]).OneResult;

                    if (!ConstantesWeb.Rol.Docente.Equals(user.Rol))
                    {
                        return this.RedirectToAction("Index", "Home");
                    }
                }

                return View(TempData["id"]);
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Crear()
        {
            if (this.Request.IsAuthenticated)
            {
                return View("Crear", TempData["id"]);                
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Consulta()
        {
            if (this.Request.IsAuthenticated)
            {
                return View("Consulta", TempData["id"]);
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        public JsonResult ConsultarSolicitudes(string numSolicitud, string estado,string codigoAlumno,string nombreAlumno,string fechaInicio, string fechaFin)
        {
            var lista = new List<Solicitud>();

            try
            {
                var solicitud = new Solicitud();
                solicitud.IdSolicitud = ConvertidorUtil.ConvertirInt32(numSolicitud);
                solicitud.Estado = string.IsNullOrEmpty(estado) ? "0" : estado;
                solicitud.CodigoAlumno = string.IsNullOrEmpty(codigoAlumno) ? "NULL" : codigoAlumno; 
                solicitud.Nombre = string.IsNullOrEmpty(nombreAlumno) ? "NULL" : nombreAlumno;
                solicitud.FechaInicio = string.IsNullOrEmpty(fechaInicio) ? "NULL" : fechaInicio;
                solicitud.FechaFin = string.IsNullOrEmpty(fechaFin) ? "NULL" : fechaFin;
                solicitud.IdAlumno = 0;
                solicitud.IdEmpleado = 0;

                if (Session["Login-Info"] != null)
                {
                    var user = ((Entity.Util.ReturnObject<Usuario>)Session["Login-Info"]).OneResult;

                    if (ConstantesWeb.Rol.Alumno.Equals(user.Rol))
                    {
                        solicitud.IdAlumno = ConvertidorUtil.ConvertirInt32(user.Alumno.Id);
                    }

                    if (ConstantesWeb.Rol.Docente.Equals(user.Rol))
                    {
                        solicitud.IdEmpleado = ConvertidorUtil.ConvertirInt32(user.Empleado.IdEmpleado);                        
                    }

                    if (ConstantesWeb.Rol.Coordinador.Equals(user.Rol))
                    {
                        solicitud.IdEmpleado = 0;
                    }
                }
                
                DateTime? dtFechaInicio = (fechaInicio.Length > 0) ? ConvertidorUtil.ConvertirDateTimeShort(fechaInicio) : null;
                DateTime? dtFechaFin = (fechaFin.Length > 0) ? ConvertidorUtil.ConvertirDateTimeShort(fechaFin) : null;
                DateTime dtFchFin;
                if(dtFechaInicio != null && dtFechaFin != null)
                {
                    dtFchFin = dtFechaFin.GetValueOrDefault();
                    var nuevaFchFin = dtFchFin.AddDays(1);

                    solicitud.FechaInicio = fechaInicio;
                    solicitud.FechaFin = ConvertidorUtil.FormatearFechaEsp(nuevaFchFin);
                }

                var objResult = oIBLSolicitud.ConsultarSolicitudes(solicitud);
                lista = objResult.OneResult;
            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detalle(object id)
        {
            if (this.Request.IsAuthenticated)
            {
                try
                {
                    
                    var solicitud = new Solicitud();
                    var sol = new Solicitud();
                    sol.IdSolicitud = ConvertidorUtil.ConvertirInt32(id);

                    if(sol.IdSolicitud.Equals(0))
                    {
                        return this.RedirectToAction("Index", "Consulta");
                    }

                    var objResult = oIBLSolicitud.ObtenerSolicitud(sol);
                    solicitud = objResult.OneResult;

                    var model = new SolicitudModel();
                    model.IdSolicitud = ConvertidorUtil.ConvertirString(solicitud.IdSolicitud);
                    model.NumeroSolicitud = solicitud.NumSolicitud;
                    model.FechaSolicitud = solicitud.StrFechRegistro;
                    model.TipoConsulta = solicitud.Canal;
                    model.PalabraClave = solicitud.PalabraClave;
                    model.Alumno = solicitud.CodigoAlumnoNombresApellidos;
                    model.Consulta = solicitud.Consulta;
                    model.Respuesta = solicitud.Solucion;

                    return View("Detalle", model);
                              
                }
                catch(Exception ex)
                {
                    return this.RedirectToAction("Index", "Consulta");
                }                
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <param name="solucion"></param>
        /// <returns></returns>
        public JsonResult EnviarSolucionSolicitud(string idSolicitud, string solucion,string crearConsulta,string nombreIntencion,string frase,string solucionIntencion)
        {
            var respuesta = string.Empty;

            try
            {
                if (crearConsulta.Equals("1"))
                {
                    var intencion = new Intencion();
                    intencion.IdDialogFlow = "NEW";
                    intencion.Nombre = nombreIntencion;
                    intencion.Respuesta = solucionIntencion;
                    var frases_ = new List<FraseEntrenamientoModel>();
                    frases_.Add(new FraseEntrenamientoModel { Descripcion = frase });
                    var resultado = new Dialogflow.DialogFlow(oIBLSolicitud).CreateIntent(intencion, frases_);
                }

                var solicitud = new Solicitud();
                solicitud.IdSolicitud = ConvertidorUtil.ConvertirInt32(idSolicitud);
                solicitud.Solucion = solucion;

                var objResult = oIBLSolicitud.EnviarSolucionSolicitud(solicitud);
                var notificacion = objResult.OneResult;

                if (notificacion != null)
                {
                    var email = new EmailUtil();
                    var sendEmail = email.NotificarSolucionConsultaAcademica(notificacion);

                    if (sendEmail)
                    {
                        respuesta = "SEND_EMAIL_OK";
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = respuesta, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ValidarIntencion(string nombreIntencion)
        {
            var respuesta = string.Empty;

            try
            {
                

                var objResult = oIBLSolicitud.ObtenerIntencion(nombreIntencion);
                var intencion = objResult.OneResult;

                if (intencion != null)
                {
                    if (nombreIntencion.Equals(intencion.Nombre))
                    {
                        respuesta = "EXISTE";
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = respuesta, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult ListarEstados()
        {
            var lista = new List<Estado>();
            
            
            if (Session["Login-Info"] != null)
            {
                var user = ((Entity.Util.ReturnObject<Usuario>)Session["Login-Info"]).OneResult;

                if (ConstantesWeb.Rol.Coordinador.Equals(user.Rol))
                {
                    lista.Add(new Estado() { IdEstado = "P", Descripcion = "Pendiente" });
                    lista.Add(new Estado() { IdEstado = "F", Descripcion = "No Resuelta Falta Información" });
                    lista.Add(new Estado() { IdEstado = "I", Descripcion = "Inválido" });
                    lista.Add(new Estado() { IdEstado = "D", Descripcion = "Derivado" });
                    lista.Add(new Estado() { IdEstado = "c", Descripcion = "Cancelado" });
                    lista.Add(new Estado() { IdEstado = "R", Descripcion = "Atendido por Derivación" });
                    lista.Add(new Estado() { IdEstado = "A", Descripcion = "Atendido" });
                }

                if (ConstantesWeb.Rol.Docente.Equals(user.Rol))
                {
                    lista.Add(new Estado() { IdEstado = "D", Descripcion = "Pendiente" });
                    lista.Add(new Estado() { IdEstado = "R" ,Descripcion = "Atendido" });                    
                }
            }

            return new JsonResult { Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}