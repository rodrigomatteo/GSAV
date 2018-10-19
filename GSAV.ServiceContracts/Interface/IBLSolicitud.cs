using GSAV.Entity.Objects;
using GSAV.Entity.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSAV.ServiceContracts.Interface
{
    public interface IBLSolicitud
    {
        ReturnObject<List<Solicitud>> ConsultarSolicitudes(Solicitud solicitud);
        ReturnObject<Solicitud> ObtenerSolicitud(Solicitud solicitud);
        ReturnObject<bool> EnviarSolucionSolicitud(Solicitud solicitud);
        ReturnObject<List<Solicitud>> ConsultarSolicitudesDashboard(Solicitud solicitud);
        ReturnObject<List<ChartCustom>> ConsultarDemandaUnidadNegocio(ChartCustom chart);
        ReturnObject<List<ChartCustom>> ConsultarDemandaTipoConsulta(ChartCustom chart);

    }
}
