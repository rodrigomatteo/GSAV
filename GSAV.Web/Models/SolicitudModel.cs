﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSAV.Web.Models
{
    public class SolicitudModel
    {
        public SolicitudModel()
        {            
            this.ReadOnly = "N";
            this.VisibleCrearConsulta = "N";
        }

        public string IdSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string Estatus { get; set; }
        public string Codigo { get; set; }
        public string Alumno { get; set; }
        public string TipoConsulta { get; set; }
        public string Consulta { get; set; }
        public string FechaAtencion { get; set; }
        public string DerivadoA { get; set; }
        public string FechaSolicitud { get; set; }
        public string PalabraClave { get; set; }
        public string Respuesta { get; set; }  
        public string ReadOnly { get; set; }
        public string IntencionConsulta { get; set; }
        public string IdIntencionPadre { get; set; }
        public string VisibleCrearConsulta { get; set; }
        public string Curso { get; set; }
        public string Actividad { get; set; }
        public string EtiquetaCurso { get; set; }
        public string EtiquetaActividad { get; set; }
    }
}