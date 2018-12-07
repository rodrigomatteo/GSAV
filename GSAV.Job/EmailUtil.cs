using GSAV.Entity.Objects;
using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;

namespace GSAV.Job.Util
{
    public class EmailUtil
    {
        public void EnviarCorreo(string emailAddressTo, string subject, string body, byte[] attachment, string fileName, string mediaType, bool isHtml = true)
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
            mmsg.Body = body;

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

            if(isHtml)
            {
                //Add view to the Email Message
                mmsg.AlternateViews.Add(htmlView);
            }

            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
            mmsg.IsBodyHtml = isHtml;

            //Correo electronico desde la que enviamos el mensaje
            mmsg.From = new System.Net.Mail.MailAddress("upc.chatbot@gmail.com");


            /*-------------------------CLIENTE DE CORREO----------------------*/

            //Creamos un objeto de cliente de correo

            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
            cliente.Host = "smtp.gmail.com";
            cliente.Port = 587;
            cliente.EnableSsl = true;
            cliente.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            cliente.UseDefaultCredentials = false;
            cliente.Credentials = new System.Net.NetworkCredential("upc.chatbot@gmail.com", "LimaPeru2018");

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

        public Tarea EnviarNotificacion(Solicitud solicitud)
        {
            var taskTemplateFolder = GetAbsolutePath("Templates", true);
            var pathInfo = new DirectoryInfo(taskTemplateFolder);

            if (!pathInfo.Exists)
                throw new Exception($"La carpeta no existe {pathInfo}");

            var templateName = pathInfo.FullName + @"\" + "SolicitudPendiente.xml";
            var tarea = new Tarea(templateName, string.Empty, DateTime.Now,
                new[] { "NOMBRE_RESPONSABLE", solicitud.NombreApePaterno },
                new[] { "DETALLE_SOLICITUD", solicitud.Consulta })
            {
                Destinatario = solicitud.EmailResponsable
            };


            return tarea;
        }

        public static string GetAbsolutePath(string path, bool useBaseDirSetting = false)
        {
            if (Directory.Exists(path) || File.Exists(path))
                return path;
            string path1 = AppDomain.CurrentDomain.BaseDirectory;
            if (!useBaseDirSetting)
                return Path.Combine(path1, path);
            string appSetting = ConfigurationManager.AppSettings["BaseDirectory"];
            if (!string.IsNullOrEmpty(appSetting))
                path1 = Path.Combine(path1, appSetting);
            return Path.Combine(path1, path);
        }
    }
}