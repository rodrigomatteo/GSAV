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
                dtFchFin.AddDays(1);

                solicitud.FechaInicio = fechaInicio;
                solicitud.FechaFin = ConvertidorUtil.FormatearFechaEsp(dtFchFin);
            }

            var objResult = oIBLSolicitud.ConsultarSolicitudesDashboard(solicitud);
            lista = objResult.OneResult;
            return lista;

        }

        public JsonResult ListarSolicitudesAtencion(string fechaInicio, string fechaFin)
        {
            var lista = new List<Solicitud>();

            try
            {
                lista = plistarSolicitudesAtencion(fechaInicio, fechaFin);
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
                    indicadoresDashboard.NoResueltos = lista.FindAll(q => q.Estado.Equals("I") || q.Estado.Equals("F") || q.Estado.Equals("C") || q.Estado.Equals("P")).Count() + "";
                    
                    indicadoresDashboard.CumplioSla = "90";
                    indicadoresDashboard.NoCumplioSla = "30";
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
                    dtFchFin.AddDays(1);

                    chart.FechaInicio = fechaInicio;
                    chart.FechaFin = ConvertidorUtil.FormatearFechaEsp(dtFchFin);
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
                    dtFchFin.AddDays(1);

                    chart.FechaInicio = fechaInicio;
                    chart.FechaFin = ConvertidorUtil.FormatearFechaEsp(dtFchFin);
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
    }
}