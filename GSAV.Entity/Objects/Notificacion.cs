using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSAV.Entity.Objects
{
    public class Notificacion
    {
        public string Email { get; set; }
        public string NombreApellidoAlumno {
            get
            {
                return Nombres + ", " + ApellidoPat;
            }
        }
        public string CodigoAlumno { get; set; }
        public string NumeroConsulta
        {
            get
            {
                return IdSolicitud.ToString().PadLeft(5, '0');
            }
        }
        public string ConsultaAcademica { get; set; }
        public string FechaConsulta {
            get
            {
                return FormatearFechaHora(DtFechaConsulta);
            }
        }
        public string FechaSolucion
        {
            get
            {
                return FormatearFechaHora(DtFechaSolucion);
            }
        }
        public string Solucion { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPat { get; set; }
        public DateTime? DtFechaConsulta { get; set; }
        public DateTime? DtFechaSolucion { get; set; }
        public static string FormatearFechaHora(DateTime? date)
        {
            var resultado = string.Empty;
            try
            {
                if (resultado != null)
                    resultado = string.Format("{0:dd/MM/yyyy HH:mm}", date);
                if (date == DateTime.MinValue)
                    resultado = string.Empty;
            }
            catch (Exception)
            {

            }
            return resultado;
        }
        public int IdSolicitud { get; set; }
    }
}
