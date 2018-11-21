using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSAV.Web.Util
{
    public static class ConstantesWeb
    {
        public static class Rol
        {
            public const string Docente = "DOCENTE";
            public const string Coordinador = "COORDINADOR";
            public const string Alumno = "ALUMNO";
            public const string Administrador = "ADMINISTRADOR";
        }

        public static class DialogFlow
        {
            public const string FilePrivateKeyIdJson = "upc-chatbot-2b629c2109dc.json";
            public const string ProjectId = "upc-chatbot";
        }
    }
}