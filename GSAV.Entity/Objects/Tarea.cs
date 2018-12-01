using System;
using System.IO;
using System.Xml;

namespace GSAV.Entity.Objects
{
    public class Tarea
    {
        public string Asunto { get; set; }
        public string Descripcion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Destinatario { get; set; }

        public Tarea(string plantilla, string creadoPor, DateTime fechaCreacion, params string[][] parametros)
        {
            var xmlConfiguration = new XmlDocument();

            var configFileInfo = new FileInfo(plantilla);
            if (!configFileInfo.Exists)
                throw new Exception($"El archivo no existe {plantilla}");

            xmlConfiguration.Load(configFileInfo.FullName);

            // subject
            var xpath = @"task/subject";
            var nSubject = xmlConfiguration.SelectSingleNode(xpath);

            if (nSubject == null)
                throw new Exception($"Seccion {xpath} no existe");
            xpath = @"task/description";

            var nDescription = xmlConfiguration.SelectSingleNode(xpath);

            Asunto = nSubject.InnerText;
            Descripcion = nDescription?.InnerText ?? throw new Exception($"Elemento {xpath} no existe");
            CreadoPor = creadoPor;
            FechaCreacion = fechaCreacion;

            if (parametros == null) return;

            foreach (var paramKey in parametros)
            {
                var paramValue = paramKey[1];
                Asunto = Asunto.Replace(paramKey[0], paramValue);
                Descripcion = Descripcion.Replace(paramKey[0], paramValue);
            }
        }
    }
}
