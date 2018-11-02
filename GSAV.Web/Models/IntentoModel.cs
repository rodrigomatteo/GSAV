using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSAV.Web.Models
{
    public class IntentoModel
    {
        public IntentoModel()
        {
            this.Id = string.Empty;            
            this.Nombre = string.Empty;
            this.FrasesEntrenamiento = new List<FraseEntrenamientoModel>();
            this.Respuestas = new List<RespuestaIntentoModel>();
        }

        public string IdShort {
            get
            {
                var idShort_ = string.Empty;
                try
                {
                    idShort_ = this.Id.Substring(0, 5);
                }catch(Exception ex)
                {
                }
                return idShort_;
            }
        }
        public string Id { get; set; }
        public string Nombre { get; set; }
        public List<FraseEntrenamientoModel> FrasesEntrenamiento { set; get; }
        public List<RespuestaIntentoModel> Respuestas { get; set; }
        public string CantidadFrases
        {
            get
            {
                return FrasesEntrenamiento.Count().ToString();
            }
        }
        public string Respuesta
        {
            get
            {
                var respuesta = string.Empty;
                try
                {
                    respuesta = Respuestas.AsEnumerable().FirstOrDefault().Descripcion;
                }catch(Exception ex)
                {

                }
                return respuesta;
            }
        }

    }
}