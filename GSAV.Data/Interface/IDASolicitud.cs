using GSAV.Entity.Objects;
using GSAV.Entity.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSAV.Data.Interface
{
    public interface IDASolicitud
    {
        ReturnObject<List<Solicitud>> ConsultarSolicitudes(Solicitud solicitud);
        ReturnObject<Solicitud> ObtenerSolicitud(Solicitud solicitud);
        ReturnObject<Notificacion> EnviarSolucionSolicitud(Solicitud solicitud);
        ReturnObject<List<Solicitud>> ConsultarSolicitudesDashboard(Solicitud solicitud);
        ReturnObject<List<ChartCustom>> ConsultarDemandaUnidadNegocio(ChartCustom chart);
        ReturnObject<List<ChartCustom>> ConsultarDemandaTipoConsulta(ChartCustom chart);
        ReturnObject<string> ObtenerFechaIntencion(string intencionNombre);
        ReturnObject<List<Intencion>> ObtenerIntenciones();
        ReturnObject<string> InsertarIntencionConsulta(string nombreIntencion, string idDialogFlow, DateTime fechaCreacion, string intencionPadre);
        ReturnObject<Intencion> ObtenerIntencion(string intencionNombre);
        ReturnObject<string> EliminarIntencionConsulta(string idDialogFlow);
    }
}
