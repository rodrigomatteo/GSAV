using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSAV.Entity.Objects
{
    public class Solicitud
    {
        public int IdSolicitud { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Canal { get; set; }
        public string Intencion { get; set; }
        public string CodigoAlumno { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPat { get; set; }
        public string ApellidoMat { get; set; }
        public string Estado { get; set; }
        public string Consulta { get; set; }
        public string Solucion { get; set; }
        public string PalabraClave { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string NombreApePaterno
        {
            get
            {
                return Nombre + ", " + ApellidoPat;
            }
        }
        public string EstadoDesc
        {
            get
            {
                var estadoDesc = "";

                switch (Estado)
                {
                    case "A":
                        estadoDesc = "Atendida";
                        break;
                    case "R":
                        estadoDesc = "Atendida por Derivación";
                        break;
                    case "C":
                        estadoDesc = "Cancelado";
                        break;
                    case "D":
                        estadoDesc = "Derivado";
                        break;
                    case "I":
                        estadoDesc = "Inválido";
                        break;
                    case "F":
                        estadoDesc = "No Resuelta";
                        break;
                    case "P":
                        estadoDesc = "Pendiente";
                        break;
                    default:
                        estadoDesc = "";
                        break;
                }
                
                return estadoDesc;
            }
        }
        public string StrFechRegistro
        {
            get
            {
                var resultado = string.Empty;
                try
                {
                    if (resultado != null)
                        resultado = string.Format("{0:dd/MM/yyyy}", FechaRegistro);
                    if (FechaRegistro == DateTime.MinValue)
                        resultado = string.Empty;
                }
                catch (Exception)
                {

                }
                return resultado;
            }
        }
        public string NumSolicitud
        {
            get
            {
                return IdSolicitud.ToString().PadLeft(5, '0');
            }
        }
        public string CodigoAlumnoNombresApellidos
        {
            get
            {
                return CodigoAlumno + " - " + Nombre + ", " + ApellidoPat + " " + ApellidoMat;
            }
        }
        public string DerivadoA { get; set; }
        public int IdEmpleado { get; set; }
        public int IdAlumno { get; set; }
    }
}
