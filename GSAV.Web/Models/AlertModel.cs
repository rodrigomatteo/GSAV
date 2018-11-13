using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSAV.Web.Models
{
    public class AlertModel
    {
        public AlertModel()
        {
            Id = string.Empty;
            Mensaje = string.Empty;
            DisplayName = string.Empty;
            MessageError = string.Empty;
        }
        public string Id { get; set; }
        public string Mensaje { get; set; }
        public string DisplayName { get; set; }
        public string MessageError { get; set; }
    }
}