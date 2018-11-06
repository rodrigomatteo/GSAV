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
        public JsonResult ConsultarIntenciones(string idIntencion,string palabraClave)
        {
            var lista = new List<IntentoModel>();

            lista =  new Dialogflow.DialogFlow(oIBLSolicitud).ObtenerIntentos();            

            return new JsonResult { Data = lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Detalle(object id)
        {
            if (this.Request.IsAuthenticated)
            {
                try
                {
                    var idIntencion = id + string.Empty;

                    var intencionModel = new IntentoModel();
                    intencionModel = Dialogflow.DialogFlow.ObtenerIntento(idIntencion);
                   
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

        public ActionResult Crear()
        {
            if (this.Request.IsAuthenticated)
            {
                try
                {
                    var intencionModel = new IntentoModel();
                    
                    return View("Crear", intencionModel);

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
    }
}