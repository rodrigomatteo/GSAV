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
    public class DASolicitud : IDASolicitud
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public ReturnObject<List<Solicitud>> ConsultarSolicitudes(Solicitud solicitud)
        {
            ReturnObject<List<Solicitud>> obj = new ReturnObject<List<Solicitud>>();
            obj.OneResult = new List<Solicitud>();

            try
            {
                using (var oCnn = Cn.OracleCn())
                {
                    OracleCommand oCmd = null;
                    oCnn.Open();
                    oCmd = new OracleCommand("SP_CONSULTAR_SOLICITUDES", oCnn);
                    oCmd.CommandType = CommandType.StoredProcedure;

                    oCmd.Parameters.Add(new OracleParameter("P_IDSOLICITUD", OracleDbType.Int64)).Value = solicitud.IdSolicitud;
                    oCmd.Parameters.Add(new OracleParameter("P_ESTADO", OracleDbType.Varchar2)).Value = solicitud.Estado;
                    oCmd.Parameters.Add(new OracleParameter("P_CODIGOALUMNO", OracleDbType.Varchar2)).Value = solicitud.CodigoAlumno;
                    oCmd.Parameters.Add(new OracleParameter("P_NOMBREALUMNO", OracleDbType.Varchar2)).Value = solicitud.Nombre;
                    oCmd.Parameters.Add(new OracleParameter("P_FECHA_INICIO", OracleDbType.Varchar2)).Value = solicitud.FechaInicio;
                    oCmd.Parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.Varchar2)).Value = solicitud.FechaFin;
                    oCmd.Parameters.Add(new OracleParameter("P_IDEMPLEADO", OracleDbType.Int64)).Value = solicitud.IdEmpleado;
                    oCmd.Parameters.Add(new OracleParameter("P_IDALUMNO", OracleDbType.Int64)).Value = solicitud.IdAlumno;
                    oCmd.Parameters.Add(new OracleParameter("P_RC", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;

                    OracleDataReader rd = oCmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            var solicitud_ = new Solicitud();
                            solicitud_.IdSolicitud = rd.GetInt32(rd.GetOrdinal("IDSOLICITUD"));
                            solicitud_.FechaRegistro = rd.GetDateTime(rd.GetOrdinal("FECHAREGISTRO"));
                            solicitud_.Canal = rd.GetValue(rd.GetOrdinal("CANAL")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CANAL"));
                            solicitud_.Intencion = rd.GetValue(rd.GetOrdinal("INTENCION")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("INTENCION"));
                            solicitud_.CodigoAlumno = rd.GetValue(rd.GetOrdinal("CODIGOALUMNO")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CODIGOALUMNO"));
                            solicitud_.Nombre = rd.GetValue(rd.GetOrdinal("NOMBRE")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("NOMBRE"));
                            solicitud_.ApellidoPat = rd.GetValue(rd.GetOrdinal("APELLIDOPAT")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("APELLIDOPAT"));
                            solicitud_.ApellidoMat = rd.GetValue(rd.GetOrdinal("APELLIDOMAT")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("APELLIDOMAT"));
                            solicitud_.Estado = rd.GetValue(rd.GetOrdinal("ESTADO")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("ESTADO"));
                            solicitud_.Consulta = rd.GetValue(rd.GetOrdinal("CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CONSULTA"));
                            solicitud_.Solucion = rd.GetValue(rd.GetOrdinal("SOLUCION")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("SOLUCION"));
                            obj.OneResult.Add(solicitud_);                           
                        }
                    }

                    obj.Success = true;

                }
            }
            catch (Exception ex)
            {
                obj.Success = false;
                obj.ErrorMessage = ex.Message;
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public ReturnObject<Solicitud> ObtenerSolicitud(Solicitud solicitud)
        {
            ReturnObject<Solicitud> obj = new ReturnObject<Solicitud>();
            obj.OneResult = new Solicitud();

            try
            {
                using (var oCnn = Cn.OracleCn())
                {
                    OracleCommand oCmd = null;
                    oCnn.Open();
                    oCmd = new OracleCommand("SP_OBTENER_SOLICITUD", oCnn);
                    oCmd.CommandType = CommandType.StoredProcedure;

                    oCmd.Parameters.Add(new OracleParameter("P_IDSOLICITUD", OracleDbType.Int64)).Value = solicitud.IdSolicitud;                  
                    oCmd.Parameters.Add(new OracleParameter("P_RC", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;

                    OracleDataReader rd = oCmd.ExecuteReader();
                    var first = 0;

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            first++;
                            if (first.Equals(1))
                            {
                                var solicitud_ = new Solicitud();
                                solicitud_.IdSolicitud = rd.GetInt32(rd.GetOrdinal("IDSOLICITUD"));
                                solicitud_.FechaRegistro = rd.GetDateTime(rd.GetOrdinal("FECHAREGISTRO"));
                                solicitud_.Canal = rd.GetString(rd.GetOrdinal("CANAL"));
                                solicitud_.CodigoAlumno = rd.GetString(rd.GetOrdinal("CODIGOALUMNO"));
                                solicitud_.Nombre = rd.GetString(rd.GetOrdinal("NOMBRE"));
                                solicitud_.ApellidoPat = rd.GetString(rd.GetOrdinal("APELLIDOPAT"));
                                solicitud_.ApellidoMat = rd.GetString(rd.GetOrdinal("APELLIDOMAT"));
                                solicitud_.Estado = rd.GetString(rd.GetOrdinal("ESTADO"));
                                solicitud_.Consulta = rd.GetValue(rd.GetOrdinal("CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CONSULTA"));
                                solicitud_.Solucion = rd.GetValue(rd.GetOrdinal("SOLUCION")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("SOLUCION"));
                                obj.OneResult = solicitud_;
                            }                            
                        }
                    }
                    obj.Success = true;
                }
            }
            catch (Exception ex)
            {
                obj.Success = false;
                obj.ErrorMessage = ex.Message;
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public ReturnObject<bool> EnviarSolucionSolicitud(Solicitud solicitud)
        {
            ReturnObject<bool> obj = new ReturnObject<bool>();
            obj.OneResult = false;

            try
            {
                using (var oCnn = Cn.OracleCn())
                {
                    OracleCommand oCmd = null;
                    oCnn.Open();
                    oCmd = new OracleCommand("SP_UPD_SOLUCION_CONSULTA", oCnn);
                    oCmd.CommandType = CommandType.StoredProcedure;

                    oCmd.Parameters.Add(new OracleParameter("P_IDSOLICITUD", OracleDbType.Int64)).Value = solicitud.IdSolicitud;
                    oCmd.Parameters.Add(new OracleParameter("P_SOLUCION", OracleDbType.Varchar2)).Value = solicitud.Solucion;
                    
                    oCmd.ExecuteNonQuery();
                   
                    obj.Success = true;
                    obj.OneResult = true;
                }
            }
            catch (Exception ex)
            {
                obj.OneResult = false;
                obj.Success = false;
                obj.ErrorMessage = ex.Message;
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        public ReturnObject<List<Solicitud>> ConsultarSolicitudesDashboard(Solicitud solicitud)
        {
            ReturnObject<List<Solicitud>> obj = new ReturnObject<List<Solicitud>>();
            obj.OneResult = new List<Solicitud>();

            try
            {
                using (var oCnn = Cn.OracleCn())
                {
                    OracleCommand oCmd = null;
                    oCnn.Open();
                    oCmd = new OracleCommand("SP_CONSULTAR_SOLICITUDES_DSH", oCnn);
                    oCmd.CommandType = CommandType.StoredProcedure;
                                     
                    oCmd.Parameters.Add(new OracleParameter("P_FECHA_INICIO", OracleDbType.Varchar2)).Value = solicitud.FechaInicio;
                    oCmd.Parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.Varchar2)).Value = solicitud.FechaFin;
                    oCmd.Parameters.Add(new OracleParameter("P_RC", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;

                    OracleDataReader rd = oCmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            var solicitud_ = new Solicitud();
                            solicitud_.IdSolicitud = rd.GetInt32(rd.GetOrdinal("IDSOLICITUD"));
                            solicitud_.FechaRegistro = rd.GetDateTime(rd.GetOrdinal("FECHAREGISTRO"));
                            solicitud_.Canal = rd.GetString(rd.GetOrdinal("CANAL"));
                            solicitud_.CodigoAlumno = rd.GetString(rd.GetOrdinal("CODIGOALUMNO"));
                            solicitud_.Nombre = rd.GetString(rd.GetOrdinal("NOMBRE"));
                            solicitud_.ApellidoPat = rd.GetString(rd.GetOrdinal("APELLIDOPAT"));
                            solicitud_.ApellidoMat = rd.GetString(rd.GetOrdinal("APELLIDOMAT"));
                            solicitud_.Estado = rd.GetString(rd.GetOrdinal("ESTADO"));
                            solicitud_.Consulta = rd.GetValue(rd.GetOrdinal("CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CONSULTA"));
                            solicitud_.Solucion = rd.GetValue(rd.GetOrdinal("SOLUCION")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("SOLUCION"));
                            obj.OneResult.Add(solicitud_);
                        }
                    }

                    obj.Success = true;

                }
            }
            catch (Exception ex)
            {
                obj.Success = false;
                obj.ErrorMessage = ex.Message;
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chart"></param>
        /// <returns></returns>
        public ReturnObject<List<ChartCustom>> ConsultarDemandaUnidadNegocio(ChartCustom chart)
        {
            ReturnObject<List<ChartCustom>> obj = new ReturnObject<List<ChartCustom>>();
            obj.OneResult = new List<ChartCustom>();

            try
            {
                using (var oCnn = Cn.OracleCn())
                {
                    OracleCommand oCmd = null;
                    oCnn.Open();
                    oCmd = new OracleCommand("SP_CONSULTAR_DEMANDA_UNID_NEG", oCnn);
                    oCmd.CommandType = CommandType.StoredProcedure;

                    oCmd.Parameters.Add(new OracleParameter("P_FECHA_INICIO", OracleDbType.Varchar2)).Value = chart.FechaInicio;
                    oCmd.Parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.Varchar2)).Value = chart.FechaFin;
                    oCmd.Parameters.Add(new OracleParameter("P_RC", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;

                    OracleDataReader rd = oCmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            var chart_ = new ChartCustom();
                            chart_.Descripcion = rd.GetString(rd.GetOrdinal("UNIDAD"));
                            chart_.Cantidad = rd.GetInt32(rd.GetOrdinal("CANTIDAD"));                           
                            obj.OneResult.Add(chart_);
                        }
                    }

                    obj.Success = true;

                }
            }
            catch (Exception ex)
            {
                obj.Success = false;
                obj.ErrorMessage = ex.Message;
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chart"></param>
        /// <returns></returns>
        public ReturnObject<List<ChartCustom>> ConsultarDemandaTipoConsulta(ChartCustom chart)
        {
            ReturnObject<List<ChartCustom>> obj = new ReturnObject<List<ChartCustom>>();
            obj.OneResult = new List<ChartCustom>();

            try
            {
                using (var oCnn = Cn.OracleCn())
                {
                    OracleCommand oCmd = null;
                    oCnn.Open();
                    oCmd = new OracleCommand("SP_CONSULTAR_DEMANDA_TIPO", oCnn);
                    oCmd.CommandType = CommandType.StoredProcedure;

                    oCmd.Parameters.Add(new OracleParameter("P_FECHA_INICIO", OracleDbType.Varchar2)).Value = chart.FechaInicio;
                    oCmd.Parameters.Add(new OracleParameter("P_FECHA_FIN", OracleDbType.Varchar2)).Value = chart.FechaFin;
                    oCmd.Parameters.Add(new OracleParameter("P_RC", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;

                    OracleDataReader rd = oCmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            var chart_ = new ChartCustom();
                            chart_.Descripcion = rd.GetString(rd.GetOrdinal("DESCRIPCION"));
                            chart_.Cantidad = rd.GetInt32(rd.GetOrdinal("CANTIDAD"));
                            obj.OneResult.Add(chart_);
                        }
                    }

                    obj.Success = true;

                }
            }
            catch (Exception ex)
            {
                obj.Success = false;
                obj.ErrorMessage = ex.Message;
            }

            return obj;
        }

        public ReturnObject<string> ObtenerFechaIntencion(string intencionNombre)
        {
            throw new NotImplementedException();
        }

        public ReturnObject<List<Intencion>> ObtenerIntenciones()
        {
            throw new NotImplementedException();
        }

        public ReturnObject<string> InsertarIntencionConsulta(string nombreIntencion, string idDialogFlow, DateTime fechaCreacion)
        {
            throw new NotImplementedException();
        }
    }
}
