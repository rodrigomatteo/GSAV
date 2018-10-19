using GSAV.Data.Common;
using GSAV.Data.Interface;
using GSAV.Entity.Objects;
using GSAV.Entity.Util;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSAV.Data.Oracle.Implementation
{
    public class DAUsuario : IDAUsuario
    {
        public ReturnObject<Usuario> ValidarAcceso(Usuario usuario)
        {
            ReturnObject<Usuario> obj = new ReturnObject<Usuario>();
            obj.OneResult = new Usuario();

            try
            {
                var user = new Usuario();
                using (var oCnn = Cn.OracleCn())
                {
                    OracleCommand oCmd = null;
                    oCnn.Open();
                    oCmd = new OracleCommand(SP.SP_VALIDARACCESO, oCnn);
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.Parameters.Add(new OracleParameter("P_USUARIO", OracleDbType.Varchar2)).Value = usuario.NombreUsuario;
                    oCmd.Parameters.Add(new OracleParameter("P_CLAVE", OracleDbType.Varchar2)).Value = usuario.Clave;
                    oCmd.Parameters.Add(new OracleParameter("P_RC", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;
                    OracleDataReader rd = oCmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        if (rd.Read())
                        {
                            user = new Usuario();
                            user.Id = rd.GetInt32(rd.GetOrdinal("IDUSUARIO"));
                            user.NombreUsuario = rd.GetString(rd.GetOrdinal("NOMBREUSUARIO"));
                            user.Rol = rd.GetString(rd.GetOrdinal("ROL"));

                            if (rd.GetValue(rd.GetOrdinal("IDALUMNO")) != DBNull.Value)
                            {
                                var alumno = new Alumno();
                                alumno.Id = rd.GetInt32(rd.GetOrdinal("IDALUMNO"));
                                alumno.Nombre = rd.GetValue(rd.GetOrdinal("NOMBRE_ALUMNO")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("NOMBRE_ALUMNO"));
                                alumno.Unidad = rd.GetValue(rd.GetOrdinal("UNIDAD")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("UNIDAD"));
                                user.Alumno = alumno;
                            }

                            if (rd.GetValue(rd.GetOrdinal("IDEMPLEADO")) != DBNull.Value)
                            {
                                var empleado = new Empleado();
                                empleado.IdEmpleado = rd.GetInt32(rd.GetOrdinal("IDEMPLEADO"));                               
                                user.Empleado = empleado;
                            }

                            var persona = new Persona();
                            persona.IdPersona = rd.GetValue(rd.GetOrdinal("IDPERSONA")) == DBNull.Value ? 0 : rd.GetInt32(rd.GetOrdinal("IDPERSONA"));
                            persona.Nombre = rd.GetValue(rd.GetOrdinal("NOMBRE_PERSONA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("NOMBRE_PERSONA"));
                            persona.ApellidoPat = rd.GetValue(rd.GetOrdinal("APELLIDOPAT_PERSONA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("APELLIDOPAT_PERSONA"));
                            user.Persona = persona;

                            obj.OneResult = user;
                            obj.Success = true;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                obj.Success = false;
                obj.ErrorMessage = ex.Message;
            }

            return obj;
        }
    }
}
