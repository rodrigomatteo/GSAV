using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSAV.Entity.Objects
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }
        public Alumno Alumno { get; set; }
        public Persona Persona { get; set; }
        public Empleado Empleado { get; set; }
        public string Rol { set; get; }
    }
}
