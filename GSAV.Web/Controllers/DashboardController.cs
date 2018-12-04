using GSAV.Entity.Objects;
using GSAV.ServiceContracts.Interface;
using GSAV.Web.Models;
using GSAV.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GSAV.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IBLSolicitud oIBLSolicitud;

        public DashboardController(IBLSolicitud bLSolicitud)
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

                    if (!ConstantesWeb.Rol.Coordinador.Equals(user.Rol) && !ConstantesWeb.Rol.Administrador.Equals(user.Rol))
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

        private List<Solicitud> plistarSolicitudesAtencion(string fechaInicio, string fechaFin)
        {
            var lista = new List<Solicitud>();
            var solicitud = new Solicitud();
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
            if (dtFechaInicio != null && dtFechaFin != null)
            {
                dtFchFin = dtFechaFin.GetValueOrDefault();
                var nuevaFchFin = dtFchFin.AddDays(1);

                solicitud.FechaInicio = fechaInicio;
                solicitud.FechaFin = ConvertidorUtil.FormatearFechaEsp(nuevaFchFin);
            }

            var objResult = oIBLSolicitud.ConsultarSolicitudesDashboard(solicitud);
            lista = objResult.OneResult;
            return lista;

        }

        public JsonResult ListarSolicitudesAtencion(string fechaInicio, string fechaFin,string indicadorStatus)
        {
            var lista = new List<Solicitud>();

            try
            {
                var plista = plistarSolicitudesAtencion(fechaInicio, fechaFin);

                lista = plista.Where(q => q.Estado.Equals("P") || q.Estado.Equals("D")).ToList();

                foreach (var solicitud in lista)
                {
                    var dateNow = ConvertidorUtil.GmtToPacific(DateTime.Now);
                    var dateSpan = dateNow - solicitud.FechaRegistro;
                    
                    if (dateSpan.Days > 0)
                    {
                        solicitud.IndicadorStatus = "ROJO";
                    }
                    else
                    {
                        if (dateSpan.Hours < 12)
                        {
                            solicitud.IndicadorStatus = "VERDE";
                        }

                        if (dateSpan.Hours < 18 && dateSpan.Hours >= 12)
                        {
                            solicitud.IndicadorStatus = "AMARILLO";
                        }

                        if (dateSpan.Hours >= 18)
                        {
                            solicitud.IndicadorStatus = "ROJO";
                        }
                    }                    
                }

                if (indicadorStatus.Equals("VERDE"))
                {
                    lista = lista.AsEnumerable().Where(q => q.IndicadorStatus.Equals("VERDE")).ToList();
                }
                if (indicadorStatus.Equals("AMARILLO"))
                {
                    lista = lista.AsEnumerable().Where(q => q.IndicadorStatus.Equals("AMARILLO")).ToList();
                }
                if (indicadorStatus.Equals("ROJO"))
                {
                    lista = lista.AsEnumerable().Where(q => q.IndicadorStatus.Equals("ROJO")).ToList();
                }

            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult CargarIndicadoresDashboard(string fechaInicio, string fechaFin)
        {
            var lst = new List<IndicadoresDashboard>();

            try
            {
                var lista = new List<Solicitud>();

                lista = plistarSolicitudesAtencion(fechaInicio, fechaFin);               
                
                var indicadoresDashboard = new IndicadoresDashboard();

                if(lista != null && lista.Any())
                {
                    indicadoresDashboard.Total = lista.Count() + "";
                    indicadoresDashboard.Resueltos = lista.FindAll(q=> q.Estado.Equals("A")).Count() + "";
                    indicadoresDashboard.Derivados = lista.FindAll(q => q.Estado.Equals("D") || q.Estado.Equals("R")).Count() + "";
                    indicadoresDashboard.Pendientes = lista.FindAll(q => q.Estado.Equals("P")).Count() + "";
                    indicadoresDashboard.NoResueltos = lista.FindAll(q => q.Estado.Equals("I") || q.Estado.Equals("F")).Count() + "";
                    indicadoresDashboard.Cancelados = lista.FindAll(q => q.Estado.Equals("C")).Count() + "";

                    indicadoresDashboard.CumplioSla = lista.FindAll(q => q.CumpleSla.Equals("1")).Count() + "";
                    indicadoresDashboard.NoCumplioSla = lista.FindAll(q => q.CumpleSla.Equals("2")).Count() + "";
                }
                
                lst.Add(indicadoresDashboard);
            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = lst, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult CargarChartDemandaUnidNegocio(string fechaInicio, string fechaFin)
        {
            var lista = new List<ChartCustom>();

            try
            {
                var chart = new ChartCustom();
                chart.FechaInicio = string.IsNullOrEmpty(fechaInicio) ? "NULL" : fechaInicio;
                chart.FechaFin = string.IsNullOrEmpty(fechaFin) ? "NULL" : fechaFin;

                DateTime? dtFechaInicio = (fechaInicio.Length > 0) ? ConvertidorUtil.ConvertirDateTimeShort(fechaInicio) : null;
                DateTime? dtFechaFin = (fechaFin.Length > 0) ? ConvertidorUtil.ConvertirDateTimeShort(fechaFin) : null;
                DateTime dtFchFin;
                if (dtFechaInicio != null && dtFechaFin != null)
                {
                    dtFchFin = dtFechaFin.GetValueOrDefault();
                    var nuevaFchFin = dtFchFin.AddDays(1);

                    chart.FechaInicio = fechaInicio;
                    chart.FechaFin = ConvertidorUtil.FormatearFechaEsp(nuevaFchFin);
                }

                var objResult = oIBLSolicitud.ConsultarDemandaUnidadNegocio(chart);
                lista = objResult.OneResult;
                
            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult CargarChartDemandaTipoConsulta(string fechaInicio, string fechaFin)
        {
            var lista = new List<ChartCustom>();

            try
            {
                var chart = new ChartCustom();
                chart.FechaInicio = string.IsNullOrEmpty(fechaInicio) ? "NULL" : fechaInicio;
                chart.FechaFin = string.IsNullOrEmpty(fechaFin) ? "NULL" : fechaFin;

                DateTime? dtFechaInicio = (fechaInicio.Length > 0) ? ConvertidorUtil.ConvertirDateTimeShort(fechaInicio) : null;
                DateTime? dtFechaFin = (fechaFin.Length > 0) ? ConvertidorUtil.ConvertirDateTimeShort(fechaFin) : null;
                DateTime dtFchFin;
                if (dtFechaInicio != null && dtFechaFin != null)
                {
                    dtFchFin = dtFechaFin.GetValueOrDefault();
                    var nuevaFchFin =  dtFchFin.AddDays(1);

                    chart.FechaInicio = fechaInicio;
                    chart.FechaFin = ConvertidorUtil.FormatearFechaEsp(nuevaFchFin);
                }

                var objResult = oIBLSolicitud.ConsultarDemandaTipoConsulta(chart);
                lista = objResult.OneResult;

            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ConsultarSolicitudAtencion(string idSolicitud)
        {
            var lista = new List<Solicitud>();

            try
            {
                var solicitud = new Solicitud();
                solicitud.IdSolicitud = ConvertidorUtil.ConvertirInt32(idSolicitud);                

                var objResult = oIBLSolicitud.ConsultarSolicitudes(solicitud);
                lista = objResult.OneResult;

            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #region Alerta

        [HttpPost]
        public ActionResult EnviarAlerta()
        {
            // Qué hace? => Envía una alerta
            // Cómo lo hace => De manera automática
            // A quién? => A los responsables de la atención de una solicitud académica o técnica
            // Qué incluye? => Solicitudes que se encuentran pendientes de atención por más de 18 horas
            // Cuándo se ejecuta? => Cada 4 horas. [Si está vacío, se ejecuta de manera inmediata]
            // Paso 1: Registrar datos en el log
            RegistrarLog("1");

            // Paso 2: Obtener solicitudes que se encuentran pendientes de atención por más de 18 horas
            var solicitudes = ObtenerSolicitudePendientesAlerta();

            // Paso 3: Validar si existen solicitudes en el paso 2
            if (solicitudes.Any())
            {
                foreach (var solicitud in solicitudes)
                    // Paso 5: Preparar solicitud
                    EnviarSolicitud(solicitud);

                // Paso 4: Actualizar la fecha de notificación de la solicitud
                ActualizarFechaNotificacion(solicitudes);
            }

            // Paso 6: Registrar en el log
            RegistrarLog("2");

            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }

        private void ActualizarFechaNotificacion(List<Solicitud> solicitudes)
        {
            oIBLSolicitud.ActualizarFechaNotificacion(solicitudes);
        }

        private void EnviarSolicitud(Solicitud solicitud)
        {
            var emailUtil = new EmailUtil();
            var tarea = emailUtil.EnviarNotificacion(solicitud);
            emailUtil.EnviarCorreo(tarea.Destinatario, tarea.Asunto, tarea.Descripcion, null, string.Empty, string.Empty, false);
        }

        private void RegistrarLog(string paso)
        {
            //throw new NotImplementedException();
        }

        private List<Solicitud> ObtenerSolicitudePendientesAlerta()
        {
            var objResult = oIBLSolicitud.ConsultarSolicitudePendientesAlerta();
            return objResult.OneResult;
        }

        #endregion

    }
}