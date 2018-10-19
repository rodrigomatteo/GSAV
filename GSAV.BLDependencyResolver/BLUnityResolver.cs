using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GSAV.BLDependencyResolver
{
    public class BLUnityResolver
    {
        public static void RegisterTypes(IUnityContainer oIUnityContainer,string database)
        {
            DADependencyRegister.RegisterTypes(oIUnityContainer,database);
            BLDependencyRegister.RegisterTypes(oIUnityContainer);
        }
    }
}
