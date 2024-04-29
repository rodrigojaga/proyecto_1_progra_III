using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace tarjetasDeCredito_proyecto1III.Models
{
    public class clsCorreo
    {
        //En esta clase se encuentra una contraseña de ejemplo y un correo
        //Sin embargo estos son solo datos de ejemplo por motivos de mi privacidad
        // si de verdad quieren probar esta funcionalidad debera conseguir por si 
        //mismo una contraseña y correo (desde Administracion de cuentas de google, debe tener la verificacion de 2 pasos activada)
        private string strPassword = "rvxx qdpc pnxm ofiu";
        private string strCorreo = "rjgs004t@gmail.com";
        private string strAlias = "Developer";

        public string fncCrearMensajeP(string strCorreoUsuario, string strSubject, string strMensaje)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage = fncCuerpoMetodo(strCorreoUsuario, strSubject, strMensaje);
                string retorno = fncEnviarMensaje(mailMessage);
                return retorno;
            } catch (Exception e) { return e.Message; }
        }

        private MailMessage fncCuerpoMetodo(string strCorreoUsuario, string strSubject, string strMensaje)
        {
            try
            {


                MailMessage correo;
                correo = new MailMessage();
                correo.From = new MailAddress(this.strCorreo, this.strAlias, System.Text.Encoding.UTF8);
                correo.To.Add(strCorreoUsuario);
                correo.Subject = strSubject;
                correo.Body = strMensaje;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.High;
                return correo;
            }
            catch (Exception)
            {
                throw new Exception("Algo salio mal al formar el correo");
            }
        }

        private string fncEnviarMensaje(MailMessage mail)
        {
            try
            {


                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential(strCorreo, strPassword);
                smtp.EnableSsl = true;
                
                smtp.Send(mail);

                return "Enviado";
            }catch (Exception e)
            {
                return e.Message;
            }
        }


    }
}
