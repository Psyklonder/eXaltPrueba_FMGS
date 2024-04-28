using System.Net.Mail;
using System.Net;
using Prueba_eXalt_FMGS.DTO;
using Prueba_eXalt_FMGS.API.InfraEstructura.Encriptacion;

namespace Prueba_eXalt_FMGS.API.InfraEstructura.EnvioCorreo
{
    public static class EnviarCorreo
    {
        public static void ConfirmarPedido(MailConfirmarPedido request)
        {
            try
            {
                var smtpClient = new SmtpClient(request.SmtpClient)
                {
                    Port = request.Port,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(EncriptarBase64.Desencriptar(request.Email), EncriptarBase64.Desencriptar(request.Pwd)),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(EncriptarBase64.Desencriptar(request.Email)),
                    Subject = request.Asunto,
                    Body = request.Mensaje,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(request.EmailDestino);

                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {

                throw e.InnerException;
            }

        }
    }
}
