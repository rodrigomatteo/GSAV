using GSAV.Entity.Objects;
using GSAV.ServiceContracts.Interface;
using GSAV.Web.Models;
using GSAV.Web.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GSAV.Web.Controllers
{
    [Authorize]
    public class BaseAcademicoController : Controller
    {

        private readonly IBLSolicitud oIBLSolicitud;

        public BaseAcademicoController(IBLSolicitud bLSolicitud)
        {
            oIBLSolicitud = bLSolicitud;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult ConsultarIntenciones(string intencion, string fraseEntrena, string respuesta, string fechaInicio, string fechaFin)
        {
            var lista = new List<IntentoModel>();
            var listaAuxFecha = new List<IntentoModel>();
            var listaAuxFrase = new List<IntentoModel>();

            lista = new Dialogflow.DialogFlow(oIBLSolicitud).ObtenerIntentos();

            if (!string.IsNullOrEmpty(respuesta))
            {
                lista = lista.Where(q => q.Respuesta.ToUpper().Contains(respuesta.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(intencion))
            {
                lista = lista.Where(q => q.Nombre.ToUpper().Contains(intencion.ToUpper())).ToList();
            }

            //-------------------------------------------------------------------------------------------------------------
            DateTime? dtFechaInicio = (fechaInicio.Length > 0) ? ConvertidorUtil.ConvertirDateTimeShort(fechaInicio) : null;
            DateTime? dtFechaFin = (fechaFin.Length > 0) ? ConvertidorUtil.ConvertirDateTimeShort(fechaFin) : null;
            DateTime dtFchFin;
            if (dtFechaInicio != null && dtFechaFin != null)
            {
                dtFchFin = dtFechaFin.GetValueOrDefault();
                var nuevaFchFin = dtFchFin.AddDays(1);

                foreach (var intentoModel in lista)
                {
                    if (intentoModel.DtFechaCreacion >= dtFechaInicio && intentoModel.DtFechaCreacion <= nuevaFchFin)
                    {
                        listaAuxFecha.Add(intentoModel);
                    }
                }

                lista = listaAuxFecha;
            }

            //-------------------------------------------------------------------------------------------------------------
            if (!string.IsNullOrEmpty(fraseEntrena))
            {
                var contieneFrase = false;
                foreach (var intentoModel in lista)
                {
                    foreach (var frase in intentoModel.FrasesEntrenamiento)
                    {
                        if (frase.Descripcion.Contains(fraseEntrena))
                        {
                            contieneFrase = true;
                        }
                    }
                    if (contieneFrase)
                    {
                        listaAuxFrase.Add(intentoModel);
                        contieneFrase = false;
                    }
                }

                lista = listaAuxFrase;

            }

            return new JsonResult { Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ConsultarFrasesEntrenamiento(string idIntencion)
        {
            var lista = new List<FraseEntrenamientoModel>();

            lista = new Dialogflow.DialogFlow().ObtenerFrasesEntrenamiento(idIntencion);

            return new JsonResult { Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Detalle(object id)
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

                try
                {
                    var intencionModel = new IntentoModel();
                    string strId = Convert.ToString(id);

                    if (strId.Equals("NEW"))
                    {
                        intencionModel.Id = "NEW";
                        intencionModel.FechaCreacion = ConvertidorUtil.FormatearFechaHora(DateTime.Now);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(strId))
                        {
                            var idIntencion = id + string.Empty;
                            intencionModel = new Dialogflow.DialogFlow(oIBLSolicitud).ObtenerIntento(idIntencion);
                        }
                    }

                    return View("Detalle", intencionModel);

                }
                catch (Exception ex)
                {
                    return this.RedirectToAction("Index", "BaseAcademico");
                }
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        public JsonResult ActualizarIntencion(string id, string nombreIntencion, string frases, string respuesta)
        {
            var resultado = new AlertModel();

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (id.Equals("NEW"))
                    {
                        var intencion = new Intencion();
                        intencion.IdDialogFlow = id;
                        intencion.Nombre = nombreIntencion;
                        intencion.Respuesta = respuesta;
                        var frases_ = JsonConvert.DeserializeObject<List<FraseEntrenamientoModel>>(frases);

                        resultado = new Dialogflow.DialogFlow(oIBLSolicitud).CreateIntent(intencion, frases_);

                    }
                    else
                    {
                        var intencion = new Intencion();
                        intencion.IdDialogFlow = id;
                        intencion.Respuesta = respuesta;
                        var frases_ = JsonConvert.DeserializeObject<List<FraseEntrenamientoModel>>(frases);

                        resultado = new Dialogflow.DialogFlow(oIBLSolicitud).UpdateIntent(intencion, frases_);

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult EliminarIntencion(string id)
        {
            var resultado = new AlertModel();

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var intencion = new Intencion();
                    intencion.IdDialogFlow = id;
                    resultado = new Dialogflow.DialogFlow(oIBLSolicitud).DeleteIntent(intencion);
                }
            }
            catch (Exception ex)
            {

            }

            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}