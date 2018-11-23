using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSAV.Web.Models
{
    public class NotificacionModel
    {
        public string Email { get; set; }
        public string NombreApellidoAlumno { get; set; }
        public string CodigoAlumno { get; set; }
        public string NumeroConsulta { get; set; }
        public string ConsultaAcademica { get; set; }
        public string FechaConsulta { get; set; }
        public string Solucion { get; set; }
    }
}