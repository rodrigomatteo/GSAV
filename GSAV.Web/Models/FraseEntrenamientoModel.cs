using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSAV.Web.Models
{
    public class FraseEntrenamientoModel
    {
        public FraseEntrenamientoModel()
        {
            this.Id = 0;           
            this.Tipo = string.Empty;
            this.Descripcion = string.Empty;
        }      
        public int Id { get; set; }
        public string StrId { get; set; }
        public string IdShort
        {
            get
            {
                var idShort_ = string.Empty;
                try
                {
                    idShort_ = this.StrId.Substring(0, 5);
                }
                catch (Exception ex)
                {
                }
                return idShort_;
            }
        }
        public string Tipo { get; set; }
        public string Descripcion { set; get; }
    }
}