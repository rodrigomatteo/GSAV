using GSAV.ServiceContracts.Implementation;
using GSAV.ServiceContracts.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GSAV.BLDependencyResolver
{
    public class BLDependencyRegister
    {
        public static void RegisterTypes(IUnityContainer oIUnityContainer)
        {
            oIUnityContainer.RegisterType<IBLUsuario, BLUsuario>();
            oIUnityContainer.RegisterType<IBLSolicitud, BLSolicitud>();
        }
    }
}
