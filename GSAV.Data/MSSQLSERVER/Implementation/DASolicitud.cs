using GSAV.Data.Common;
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
                            solicitud_.IntencionActual = rd.GetValue(rd.GetOrdinal("INTENCION_CURRENT")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("INTENCION_CURRENT"));
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
                                solicitud_.IntencionConsulta = rd.GetValue(rd.GetOrdinal("INTENCION_CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("INTENCION_CONSULTA"));
                                solicitud_.IdIntencionPadre = rd.GetValue(rd.GetOrdinal("IDINTENCIONCONSULTA_PADRE")) == DBNull.Value ? string.Empty : rd.GetInt32(rd.GetOrdinal("IDINTENCIONCONSULTA_PADRE")) + string.Empty;
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
        public ReturnObject<Notificacion> EnviarSolucionSolicitud(Solicitud solicitud)
        {
            ReturnObject<Notificacion> obj = new ReturnObject<Notificacion>();
            

            try
            {
                var notificacion = new Notificacion();
                obj.OneResult = notificacion;

                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_UPD_SOLUCION_CONSULTA, cnn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@P_IDSOLICITUD", solicitud.IdSolicitud);
                    cmd.Parameters.AddWithValue("@P_SOLUCION", solicitud.Solucion);
                    cmd.Parameters.AddWithValue("@P_ESTADO", "R");
                    cmd.Parameters.AddWithValue("@P_FECHA_SOL", GmtToPacific(DateTime.Now));
                    cmd.Parameters.AddWithValue("@P_CUMPLE_SLA", "1");                    

                    SqlDataReader rd = cmd.ExecuteReader();

                    var first = 0;

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            first++;
                            if (first.Equals(1))
                            {                               
                                notificacion.IdSolicitud = rd.GetInt32(rd.GetOrdinal("IDSOLICITUD"));
                                notificacion.Email = rd.GetValue(rd.GetOrdinal("EMAIL")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("EMAIL"));
                                notificacion.Nombres = rd.GetValue(rd.GetOrdinal("NOMBRE")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("NOMBRE"));
                                notificacion.CodigoAlumno = rd.GetValue(rd.GetOrdinal("CODIGOALUMNO")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CODIGOALUMNO"));                                
                                notificacion.ApellidoPat = rd.GetValue(rd.GetOrdinal("APELLIDOPAT")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("APELLIDOPAT"));
                                notificacion.ConsultaAcademica = rd.GetValue(rd.GetOrdinal("CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CONSULTA"));
                                notificacion.Solucion = rd.GetValue(rd.GetOrdinal("SOLUCION")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("SOLUCION"));

                                if (rd.GetValue(rd.GetOrdinal("FECHAREGISTRO")) != DBNull.Value)
                                    notificacion.DtFechaConsulta = rd.GetDateTime(rd.GetOrdinal("FECHAREGISTRO"));

                                if (rd.GetValue(rd.GetOrdinal("FECHASOLUCION")) != DBNull.Value)
                                    notificacion.DtFechaSolucion = rd.GetDateTime(rd.GetOrdinal("FECHASOLUCION"));

                                notificacion.NombreDocente = rd.GetValue(rd.GetOrdinal("NOMBRE_DOCENTE")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("NOMBRE_DOCENTE"));
                                notificacion.ApellidoPaternoDocente = rd.GetValue(rd.GetOrdinal("APEPAT_DOCENTE")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("APEPAT_DOCENTE"));
                                notificacion.NombreCurso = rd.GetValue(rd.GetOrdinal("NOMBRE_CURSO")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("NOMBRE_CURSO"));
                            }
                        }
                    }

                    obj.Success = true;
                    obj.OneResult = notificacion;
                }
            }
            catch (Exception ex)
            {
                obj.OneResult = null;
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
                            solicitud_.IntencionActual = rd.GetValue(rd.GetOrdinal("INTENCION_CURRENT")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("INTENCION_CURRENT"));
                            solicitud_.CodigoAlumno = rd.GetValue(rd.GetOrdinal("CODIGOALUMNO")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CODIGOALUMNO"));
                            solicitud_.Nombre = rd.GetValue(rd.GetOrdinal("NOMBRE")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("NOMBRE"));
                            solicitud_.ApellidoPat = rd.GetValue(rd.GetOrdinal("APELLIDOPAT")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("APELLIDOPAT"));
                            solicitud_.ApellidoMat = rd.GetValue(rd.GetOrdinal("APELLIDOMAT")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("APELLIDOMAT"));
                            solicitud_.Estado = rd.GetValue(rd.GetOrdinal("ESTADO")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("ESTADO"));
                            solicitud_.Consulta = rd.GetValue(rd.GetOrdinal("CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CONSULTA"));
                            solicitud_.CumpleSla = rd.GetValue(rd.GetOrdinal("CUMPLESLA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CUMPLESLA"));
                            var  nombreEmpleado = rd.GetValue(rd.GetOrdinal("NOMBRE_EMPLEADO")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("NOMBRE_EMPLEADO"));
                            var apellidoEmpleado = rd.GetValue(rd.GetOrdinal("APEPATERNO_EMPLEADO")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("APEPATERNO_EMPLEADO"));
                            solicitud_.DerivadoA = string.Format("{0}, {1}", nombreEmpleado, apellidoEmpleado);

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

                    cmd = new SqlCommand(SP.GSAV_SP_CONSULTAR_DEMANDA_INTENCION, cnn);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idDialogFlow"></param>
        /// <returns></returns>
        public ReturnObject<string> ObtenerFechaIntencion(string intencionNombre)
        {
            ReturnObject<string> obj = new ReturnObject<string>();
            obj.OneResult = string.Empty;

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_BUSCAR_FECHA_CREA_INTENCION, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@P_INTENCION_NOMBRE", intencionNombre);


                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            var fechaCreacion_ = rd.GetDateTime(rd.GetOrdinal("FECHACREACION"));
                            obj.OneResult = FormatearFechaEsp(fechaCreacion_);
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

        public ReturnObject<List<Intencion>> ObtenerIntenciones()
        {
            ReturnObject<List<Intencion>> obj = new ReturnObject<List<Intencion>>();
            obj.OneResult = new List<Intencion>();

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_BUSCAR_INTENCIONES, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            var intencion = new Intencion();
                            intencion.IdIntencionConsulta = rd.GetInt32(rd.GetOrdinal("IdIntencionConsulta"));
                            intencion.Nombre = (rd.GetValue(rd.GetOrdinal("Nombre")) == DBNull.Value) ? string.Empty : rd.GetString(rd.GetOrdinal("Nombre"));
                            intencion.IdPadreIntencion = (rd.GetValue(rd.GetOrdinal("IdPadreIntencion")) == DBNull.Value) ? 0 : rd.GetInt32(rd.GetOrdinal("IdPadreIntencion"));
                            intencion.IdDialogFlow = (rd.GetValue(rd.GetOrdinal("IdDialogFlow")) == DBNull.Value) ? string.Empty : rd.GetString(rd.GetOrdinal("IdDialogFlow"));
                            intencion.FechaCreacion = rd.GetDateTime(rd.GetOrdinal("FechaCreacion"));
                            intencion.StrFechaCreacion = FormatearFechaEsp(intencion.FechaCreacion);
                            obj.OneResult.Add(intencion);
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

        public ReturnObject<string> InsertarIntencionConsulta(string nombreIntencion, string idDialogFlow,DateTime fechaCreacion,string idIntencionPadre)
        {
            ReturnObject<string> obj = new ReturnObject<string>();
            obj.OneResult = string.Empty;

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_INSERTAR_INTENCION_CONSULTA, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@P_INTENCION_NOMBRE", nombreIntencion);
                    cmd.Parameters.AddWithValue("@P_ID_DIALOG_FLOW", idDialogFlow);
                    cmd.Parameters.AddWithValue("@P_FECHA_CREACION", fechaCreacion);
                    cmd.Parameters.AddWithValue("@P_INTENCION_PADRE", idIntencionPadre);

                    cmd.ExecuteNonQuery();
                  
                    obj.Success = true;
                    obj.OneResult = "INSERT OK";
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
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatearFechaEsp(DateTime date)
        {
            var resultado = string.Empty;
            try
            {
                if (resultado != null)
                    resultado = string.Format("{0:dd/MM/yyyy HH:mm}", date);
                if (date == DateTime.MinValue)
                    resultado = string.Empty;
            }
            catch (Exception)
            {

            }
            return resultado;
        }

        public static DateTime GmtToPacific(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intencionNombre"></param>
        /// <returns></returns>
        public ReturnObject<Intencion> ObtenerIntencion(string intencionNombre)
        {
            ReturnObject<Intencion> obj = new ReturnObject<Intencion>();
            obj.OneResult = new Intencion();

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_BUSCAR_INTENCION_X_NOMBRE, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@P_INTENCION_NOMBRE", intencionNombre);


                    SqlDataReader rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            var intencion = new Intencion();
                            intencion.IdIntencionConsulta = rd.GetInt32(rd.GetOrdinal("IDINTENCIONCONSULTA"));
                            intencion.Nombre = (rd.GetValue(rd.GetOrdinal("NOMBRE")) == DBNull.Value) ? string.Empty : rd.GetString(rd.GetOrdinal("NOMBRE"));
                            intencion.IdPadreIntencion = (rd.GetValue(rd.GetOrdinal("IDPADREINTENCION")) == DBNull.Value) ? 0 : rd.GetInt32(rd.GetOrdinal("IDPADREINTENCION"));
                            intencion.IdDialogFlow = (rd.GetValue(rd.GetOrdinal("IDDIALOGFLOW")) == DBNull.Value) ? string.Empty : rd.GetString(rd.GetOrdinal("IDDIALOGFLOW"));
                            intencion.FechaCreacion = rd.GetDateTime(rd.GetOrdinal("FECHACREACION"));
                            intencion.StrFechaCreacion = FormatearFechaEsp(intencion.FechaCreacion);
                            intencion.DescripcionIntencionPadre = (rd.GetValue(rd.GetOrdinal("DESC_INTENCION_PADRE")) == DBNull.Value) ? string.Empty : rd.GetString(rd.GetOrdinal("DESC_INTENCION_PADRE"));
                            obj.OneResult = intencion;
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

        public ReturnObject<string> EliminarIntencionConsulta(string idDialogFlow)
        {
            ReturnObject<string> obj = new ReturnObject<string>();
            obj.OneResult = string.Empty;

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_ELIMINAR_INTENCION_X_ID_DIALOG_FLOW, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@P_ID_DIALOG_FLOW", idDialogFlow);                  

                    cmd.ExecuteNonQuery();

                    obj.Success = true;
                    obj.OneResult = "DELETE-OK";
                }
            }
            catch (Exception ex)
            {
                obj.Success = false;
                obj.ErrorMessage = ex.Message;
            }

            return obj;
        }

        public ReturnObject<List<Solicitud>> ConsultarSolicitudePendientesAlerta()
        {
            var obj = new ReturnObject<List<Solicitud>>
            {
                OneResult = new List<Solicitud>()
            };

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_CONSULTAR_SOLICITUDES_PENDIENTES_ALERTA, cnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    var rd = cmd.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            var solicitud = new Solicitud
                            {
                                IdSolicitud = rd.GetInt32(rd.GetOrdinal("idsolicitud")),
                                Nombre = rd.GetString(rd.GetOrdinal("NOMBRE_RESPONSABLE")),
                                ApellidoPat = rd.GetString(rd.GetOrdinal("PATERNO_RESPONSABLE")),
                                Consulta = rd.GetString(rd.GetOrdinal("CONSULTA")),
                                EmailResponsable = rd.GetString(rd.GetOrdinal("EMAIL_RESPONSABLE"))
                            };
                            obj.OneResult.Add(solicitud);
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

        public void ActualizarFechaNotificacion(List<Solicitud> solicitudes)
        {
            var cnn = MSSQLSERVERCnx.MSSqlCnx();

            try
            {
                using (cnn)
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    try
                    {
                        foreach (var solicitud in solicitudes)
                        {
                            cmd = new SqlCommand(SP.GSAV_SP_UPD_FECHA_NOTIFICACION_SOLICITUD, cnn)
                            {
                                CommandType = CommandType.StoredProcedure
                            };

                            cmd.Parameters.AddWithValue("@P_IDSOLICITUD", solicitud.IdSolicitud);
                            cmd.Parameters.AddWithValue("@P_FECHA_NOTIFICACION", DateTime.Now);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch
                    {
                        // do nothing...just continue silently...shhhhhh...
                    }
                }
            }
            catch
            {
                // do nothing...just continue silently...shhhhhh...
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
            }
        }

    }
}
