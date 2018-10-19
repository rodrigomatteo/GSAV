using GSAV.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GSAV.BLDependencyResolver
{
    public class DADependencyRegister
    {
        public static void RegisterTypes(IUnityContainer oIUnityContainer,string database)
        {
            if(database.Equals("ORACLE"))
            {
                oIUnityContainer.RegisterType<IDAUsuario, GSAV.Data.Oracle.Implementation.DAUsuario>();
                oIUnityContainer.RegisterType<IDASolicitud, GSAV.Data.Oracle.Implementation.DASolicitud>();
            }

            if (database.Equals("MSSQLSERVER"))
            {
                oIUnityContainer.RegisterType<IDAUsuario, GSAV.Data.MSSQLSERVER.Implementation.DAUsuario>();
                oIUnityContainer.RegisterType<IDASolicitud, GSAV.Data.MSSQLSERVER.Implementation.DASolicitud>();
            }
        }
    }
}
