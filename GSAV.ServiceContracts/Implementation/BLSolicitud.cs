using GSAV.Data.Interface;
using GSAV.Entity.Objects;
using GSAV.Entity.Util;
using GSAV.ServiceContracts.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSAV.ServiceContracts.Implementation
{
    public class BLSolicitud : IBLSolicitud
    {
        private readonly IDASolicitud oIDASolicitud;

        public BLSolicitud(IDASolicitud p_oIDASolicitud)
        {
            oIDASolicitud = p_oIDASolicitud;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public ReturnObject<List<Solicitud>> ConsultarSolicitudes(Solicitud solicitud)
        {
            try
            {
                return oIDASolicitud.ConsultarSolicitudes(solicitud);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public ReturnObject<Notificacion> EnviarSolucionSolicitud(Solicitud solicitud)
        {
            try
            {
                return oIDASolicitud.EnviarSolucionSolicitud(solicitud);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public ReturnObject<Solicitud> ObtenerSolicitud(Solicitud solicitud)
        {
            try
            {
                return oIDASolicitud.ObtenerSolicitud(solicitud);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public ReturnObject<List<Solicitud>> ConsultarSolicitudesDashboard(Solicitud solicitud)
        {
            try
            {
                return oIDASolicitud.ConsultarSolicitudesDashboard(solicitud);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chart"></param>
        /// <returns></returns>
        public ReturnObject<List<ChartCustom>> ConsultarDemandaUnidadNegocio(ChartCustom chart)
        {
            try
            {
                return oIDASolicitud.ConsultarDemandaUnidadNegocio(chart);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chart"></param>
        /// <returns></returns>
        public ReturnObject<List<ChartCustom>> ConsultarDemandaTipoConsulta(ChartCustom chart)
        {
            try
            {
                return oIDASolicitud.ConsultarDemandaTipoConsulta(chart);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReturnObject<string> ObtenerFechaIntencion(string intencionNombre)
        {
            try
            {
                return oIDASolicitud.ObtenerFechaIntencion(intencionNombre);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReturnObject<List<Intencion>> ObtenerIntenciones()
        {
            try
            {
                return oIDASolicitud.ObtenerIntenciones();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReturnObject<string> InsertarIntencionConsulta(string nombreIntencion, string idDialogFlow, DateTime fechaCreacion, string idIntencionPadre)
        {
            try
            {
                return oIDASolicitud.InsertarIntencionConsulta(nombreIntencion, idDialogFlow, fechaCreacion,idIntencionPadre);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReturnObject<Intencion> ObtenerIntencion(string intencionNombre)
        {
            try
            {
                return oIDASolicitud.ObtenerIntencion(intencionNombre);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReturnObject<string> EliminarIntencionConsulta(string idDialogFlow)
        {
            try
            {
                return oIDASolicitud.EliminarIntencionConsulta(idDialogFlow);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReturnObject<List<Solicitud>> ConsultarSolicitudePendientesAlerta()
        {
            try
            {
                return oIDASolicitud.ConsultarSolicitudePendientesAlerta();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void ActualizarFechaNotificacion(List<Solicitud> solicitudes)
        {
            try
            {
                oIDASolicitud.ActualizarFechaNotificacion(solicitudes);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ReturnObject<List<Intencion>> ObtenerIntencionesFrecuentes()
        {            
            try
            {
                return oIDASolicitud.ObtenerIntencionesFrecuentes();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
