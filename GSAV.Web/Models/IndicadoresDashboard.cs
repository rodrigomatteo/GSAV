using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSAV.Web.Models
{
    public class IndicadoresDashboard
    {
        public IndicadoresDashboard()
        {
            Total = "0";
            Resueltos = "0";
            Derivados = "0";
            NoResueltos = "0";
            CumplioSla = "0";
            NoCumplioSla = "0";
        }

        public string Total { get; set; }
        public string Resueltos { get; set; }
        public string Derivados { get; set; }
        public string NoResueltos { get; set; }
        public string Cancelado { get; set; }
        public string CumplioSla { get; set; }
        public string NoCumplioSla { get; set; }
    }
}