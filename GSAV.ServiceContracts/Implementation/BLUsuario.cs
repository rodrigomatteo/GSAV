using GSAV.Data.Interface;
using GSAV.Entity.Objects;
using GSAV.Entity.Util;
using GSAV.ServiceContracts.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSAV.ServiceContracts.Implementation
{
    public class BLUsuario : IBLUsuario
    {
        private readonly IDAUsuario oIDAUsuario;

        public BLUsuario(IDAUsuario oIDAUsuarioIn)
        {
            oIDAUsuario = oIDAUsuarioIn;
        }

        public ReturnObject<Usuario> ValidarAcceso(Usuario usuario)
        {
            try
            {
                return oIDAUsuario.ValidarAcceso(usuario);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
