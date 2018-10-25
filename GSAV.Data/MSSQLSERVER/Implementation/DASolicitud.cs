﻿using GSAV.Data.Common;
using GSAV.Data.Interface;
using GSAV.Entity.Objects;
using GSAV.Entity.Util;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GSAV.Data.MSSQLSERVER.Implementation
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
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_CONSULTAR_SOLICITUDES, cnn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@P_IDSOLICITUD", solicitud.IdSolicitud == 0 ? 0 : solicitud.IdSolicitud);
                    cmd.Parameters.AddWithValue("@P_ESTADO", solicitud.Estado == null ? "0" : solicitud.Estado);
                    cmd.Parameters.AddWithValue("@P_CODIGOALUMNO", solicitud.CodigoAlumno == null ? "NULL" : solicitud.CodigoAlumno);
                    cmd.Parameters.AddWithValue("@P_NOMBREALUMNO", solicitud.Nombre == null ? "NULL" : solicitud.Nombre);
                    cmd.Parameters.AddWithValue("@P_FECHA_INICIO", solicitud.FechaInicio == null ? "NULL" : solicitud.FechaInicio);
                    cmd.Parameters.AddWithValue("@P_FECHA_FIN", solicitud.FechaFin == null ? "NULL" : solicitud.FechaFin);
                    cmd.Parameters.AddWithValue("@P_IDEMPLEADO", solicitud.IdEmpleado == 0 ? 0 : solicitud.IdEmpleado);
                    cmd.Parameters.AddWithValue("@P_IDALUMNO", solicitud.IdAlumno == 0 ? 0 : solicitud.IdAlumno);



                    SqlDataReader rd = cmd.ExecuteReader();

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
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_OBTENER_SOLICITUD, cnn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@P_IDSOLICITUD", solicitud.IdSolicitud);

                    SqlDataReader rd = cmd.ExecuteReader();

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
                                solicitud_.Canal = rd.GetValue(rd.GetOrdinal("CANAL")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CANAL"));
                                solicitud_.Intencion = rd.GetValue(rd.GetOrdinal("INTENCION")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("INTENCION"));
                                solicitud_.CodigoAlumno = rd.GetValue(rd.GetOrdinal("CODIGOALUMNO")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CODIGOALUMNO"));
                                solicitud_.Nombre = rd.GetValue(rd.GetOrdinal("NOMBRE")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("NOMBRE"));
                                solicitud_.ApellidoPat = rd.GetValue(rd.GetOrdinal("APELLIDOPAT")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("APELLIDOPAT"));
                                solicitud_.ApellidoMat = rd.GetValue(rd.GetOrdinal("APELLIDOMAT")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("APELLIDOMAT"));
                                solicitud_.Estado = rd.GetValue(rd.GetOrdinal("ESTADO")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("ESTADO"));
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
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_UPD_SOLUCION_CONSULTA, cnn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@P_IDSOLICITUD", solicitud.IdSolicitud);
                    cmd.Parameters.AddWithValue("@P_SOLUCION", solicitud.Solucion);

                    cmd.ExecuteNonQuery();

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
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_CONSULTAR_SOLICITUDES_DSH, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@P_FECHA_INICIO", solicitud.FechaInicio);
                    cmd.Parameters.AddWithValue("@P_FECHA_FIN", solicitud.FechaFin);

                    SqlDataReader rd = cmd.ExecuteReader();
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
                            solicitud_.CumpleSla = rd.GetValue(rd.GetOrdinal("CUMPLESLA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CUMPLESLA"));
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
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_CONSULTAR_DEMANDA_UNID_NEG, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@P_FECHA_INICIO", chart.FechaInicio);
                    cmd.Parameters.AddWithValue("@P_FECHA_FIN", chart.FechaFin);

                    SqlDataReader rd = cmd.ExecuteReader();
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
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_CONSULTAR_DEMANDA_TIPO, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@P_FECHA_INICIO", chart.FechaInicio);
                    cmd.Parameters.AddWithValue("@P_FECHA_FIN", chart.FechaFin);

                    SqlDataReader rd = cmd.ExecuteReader();
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
    }
}
