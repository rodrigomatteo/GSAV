using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSAV.Entity.Objects
{
    public class Intencion
    {
        public Intencion()
        {
            this.IdIntencionConsulta = 0;
            this.Nombre = string.Empty;
            this.IdPadreIntencion = 0;
            this.IdDialogFlow = string.Empty;
            this.FechaCreacion = DateTime.Now;
            this.StrFechaCreacion = string.Empty;
        }

        public int IdIntencionConsulta { get; set; }
        public string Nombre { get; set; }    
        public int IdPadreIntencion { get; set; }
        public string DescripcionIntencionPadre { get; set; }
        public string IdDialogFlow { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string StrFechaCreacion { get; set; }
        public string Respuesta { get; set; }
        public string IdIntencionPadre { get; set; }
        public string ValidacionIntencion { get; set; }
        
    }
}
