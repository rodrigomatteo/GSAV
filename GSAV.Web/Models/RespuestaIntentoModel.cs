using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSAV.Web.Models
{
    public class RespuestaIntentoModel
    {
        public RespuestaIntentoModel()
        {
            this.Id = string.Empty;
            this.Descripcion = string.Empty;
        }
        public string Id { get; set; }
        public string Descripcion { set; get; }
    }
}