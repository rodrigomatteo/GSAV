using GSAV.Entity.Objects;
using GSAV.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    sbBody.AppendLine("<br/>");
                    sbBody.AppendLine("<br/>");
                    sbBody.AppendLine("Estimado(a) " + notificacion.NombreApellidoAlumno + " (" + notificacion.CodigoAlumno + ")<br/>");
                    sbBody.AppendLine("<br/>");
                    sbBody.AppendLine("Por medio de la presente, se hace de conocimiento la respuesta a su consulta:" + "<br/>");
                    sbBody.AppendLine("<br/>");
                    sbBody.AppendLine("Consulta: " + notificacion.ConsultaAcademica + "<br/>");
                    sbBody.AppendLine("Fecha Consulta: " + notificacion.FechaConsulta + "<br/>");
                    sbBody.AppendLine("<br/>");
                    sbBody.AppendLine("Solución: " + notificacion.Solucion + "<br/>");
                    sbBody.AppendLine("Fecha Solución: " + notificacion.FechaSolucion + "<br/>");
                    sbBody.AppendLine("<br/>");
                    sbBody.AppendLine("Si usted tiene alguna duda o consulta puede responder al presente correo y lo atenderemos lo mas pronto posible." + "<br/>");
                    sbBody.AppendLine("<br/>");
                    sbBody.AppendLine("Atentamente" + "<br/>");
                    sbBody.AppendLine("" + "<br/>");
                    sbBody.AppendLine("Secretaría Académica" + "<br/>");
                    sbBody.AppendLine("" + "<br/>");
                    sbBody.AppendLine("Universidad Peruana de Ciencias Aplicadas (UPC)" + "<br/>");

                    EnviarCorreo(notificacion.Email, subject, sbBody.ToString(), null, string.Empty, string.Empty);
                    resultado = true;

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
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
            mmsg.Body = body;
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