using GSAV.Data.Common;
using GSAV.Data.Interface;
using GSAV.Entity.Objects;
using GSAV.Entity.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace GSAV.Data.MSSQLSERVER.Implementation
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

                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_VALIDARACCESO, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@P_USUARIO", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@P_CLAVE", usuario.Clave);

                    SqlDataReader rd = cmd.ExecuteReader();

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
