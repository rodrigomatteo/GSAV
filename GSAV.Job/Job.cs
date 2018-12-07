using GSAV.Entity.Objects;
using GSAV.Job.Util;
using GSAV.ServiceContracts.Interface;
using System.Collections.Generic;
using System.Linq;

namespace GSAV.Job
{
    public class Job
    {
        private readonly IBLSolicitud oIBLSolicitud;

        public void EnviarAlerta()
        {
            try
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
            }
            catch
            {
            }
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

        private void EnviarSolicitud(Solicitud solicitud)
        {
            var emailUtil = new EmailUtil();
            var tarea = emailUtil.EnviarNotificacion(solicitud);
            emailUtil.EnviarCorreo(tarea.Destinatario, tarea.Asunto, tarea.Descripcion, null, string.Empty, string.Empty, false);
        }

        private void ActualizarFechaNotificacion(List<Solicitud> solicitudes)
        {
            oIBLSolicitud.ActualizarFechaNotificacion(solicitudes);
        }
    }
}
