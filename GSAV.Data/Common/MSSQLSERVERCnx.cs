using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace GSAV.Data.Common
{
    public class MSSQLSERVERCnx
    {
        public static string cnxMSSQLSERVER;

        static MSSQLSERVERCnx()
        {
            cnxMSSQLSERVER = ConfigurationManager.ConnectionStrings["GSAV_MSSQLSERVER"].ToString();
        }

        public static SqlConnection MSSqlCnx() => new SqlConnection(cnxMSSQLSERVER);
    }
}
