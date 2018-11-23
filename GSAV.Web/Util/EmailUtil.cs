using GSAV.Entity.Objects;
using GSAV.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GSAV.Web.Util
{
    public class EmailUtil
    {


        public bool NotificarSolucionConsultaAcademica(Notificacion notificacion)
        {
            var resultado = false;
            try
            {

                string emailAddressTo = string.Empty;
                string subject = string.Empty;

                /*Enviar correo*/

                subject = string.Format("Notificación - Consulta Académica {0} {1}", notificacion.NumeroConsulta, notificacion.FechaSolucion);

                var sbBody = new StringBuilder();
                //sbBody.AppendLine("<br/>");
                sbBody.AppendLine("<br/>");
                //sbBody.AppendLine("Estimado(a) " + notificacion.NombreApellidoAlumno + " (" + notificacion.CodigoAlumno + ")<br/>");
                sbBody.AppendLine("Estimado Alumno:" + "<br/>");
                sbBody.AppendLine("<br/>");
                sbBody.AppendLine(GetSaludoHora(ConvertidorUtil.GmtToPacific(DateTime.Now)) + "a continuación, se remite la respuesta a su consulta:" + "<b>" + notificacion.ConsultaAcademica + "</b>" + "</br>");
                sbBody.AppendLine("<br/>");
                //sbBody.AppendLine("Consulta: " + notificacion.ConsultaAcademica + "<br/>");
                //sbBody.AppendLine("Fecha Consulta: " + notificacion.FechaConsulta + "<br/>");
                sbBody.AppendLine("<br/>");
                sbBody.AppendLine("<u><b>Respuesta:</b></u>" + notificacion.Solucion + "<br/>");
                //sbBody.AppendLine("Fecha Solución: " + notificacion.FechaSolucion + "<br/>");
                sbBody.AppendLine("<br/>");
                //sbBody.AppendLine("Si usted tiene alguna duda o consulta puede responder al presente correo y lo atenderemos lo mas pronto posible." + "<br/>");
                sbBody.AppendLine("<br/>");
                sbBody.AppendLine("Saludos Cordiales," + "<br/>");
                sbBody.AppendLine("<table><tr><td> <img src=cid:myImageID> </td>");
                sbBody.AppendLine("<td><table><tr><td>" + notificacion.NombreApellidoDocente + "</td></tr><tr><td>Docente</td></tr><tr><td>" +  notificacion.NombreCurso +"</td></tr></table></td></tr></table>");
                sbBody.AppendLine("" + "<br/>");
                //sbBody.AppendLine("Universidad Peruana de Ciencias Aplicadas (UPC)" + "<br/>");

                EnviarCorreo(notificacion.Email, subject, sbBody.ToString(), null, string.Empty, string.Empty);
                resultado = true;


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
        }

        private string GetSaludoHora(DateTime dt)
        {
            var saludo = string.Empty;

            if (dt.Hour >= 0 && dt.Hour < 12)
            {
                saludo = "Buenos días,";
            }
            else if (dt.Hour >= 12 && dt.Hour < 18)
            {
                saludo = "Buenas tardes,";
            }
            else
            {
                saludo = "Buenas noches,";
            }

            return saludo;
        }

        private void EnviarCorreo(string emailAddressTo, string subject, string body, byte[] attachment, string fileName, string mediaType)
        {
            /*-------------------------MENSAJE DE CORREO----------------------*/

            //Creamos un nuevo Objeto de mensaje
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();

            //Direccion de correo electronico a la que queremos enviar el mensaje
            mmsg.To.Add(emailAddressTo);

            //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario


            //Asunto
            mmsg.Subject = subject;
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

            //Attachment

            //save the data to a memory stream
            if (attachment != null)
            {
                MemoryStream ms = new MemoryStream(attachment);

                //create the attachment from a stream. Be sure to name the data 
                //with a file and 
                //media type that is respective of the data
                //mmsg.Attachments.Add(new System.Net.Mail.Attachment(ms, "Reporte.pdf", "application/pdf"));
                mmsg.Attachments.Add(new System.Net.Mail.Attachment(ms, fileName, mediaType));
            }


            //Cuerpo del Mensaje

            //create Alrternative HTML view
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

            //Add Image
            var fileSavePath = HttpContext.Current.Server.MapPath("~/Content/images/") + "upc_logo.png";
            FileInfo fileInfo = new FileInfo(fileSavePath);
            byte[] file = File.ReadAllBytes(fileSavePath);

            if (fileInfo.Exists)
            {
                using (var stream = new MemoryStream(file))
                {
                    LinkedResource theEmailImage = new LinkedResource(stream);
                    theEmailImage.ContentId = "myImageID";

                    //Add the Image to the Alternate view
                    htmlView.LinkedResources.Add(theEmailImage);

                    //Add view to the Email Message
                    mmsg.AlternateViews.Add(htmlView);


                    mmsg.BodyEncoding = System.Text.Encoding.UTF8;
                    mmsg.IsBodyHtml = true;


                    //Correo electronico desde la que enviamos el mensaje
                    mmsg.From = new System.Net.Mail.MailAddress(ConstantesWeb.Email.Administrador);


                    /*-------------------------CLIENTE DE CORREO----------------------*/

                    //Creamos un objeto de cliente de correo

                    System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
                    cliente.Host = "smtp.gmail.com";
                    cliente.Port = 587;
                    cliente.EnableSsl = true;
                    cliente.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    cliente.UseDefaultCredentials = false;
                    cliente.Credentials = new System.Net.NetworkCredential(ConstantesWeb.Email.Administrador, ConstantesWeb.Email.Password);

                    /*-------------------------ENVIO DE CORREO----------------------*/

                    try
                    {
                        //Enviamos el mensaje      
                        cliente.Send(mmsg);
                    }
                    catch (System.Net.Mail.SmtpException ex)
                    {
                        //Aquí gestionamos los errores al intentar enviar el correo
                        throw ex;
                    }

                }
            }            
        }
    }
}