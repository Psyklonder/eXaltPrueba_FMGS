using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_eXalt_FMGS.DTO
{
    public class MailConfirmarPedido
    {
        public string SmtpClient { get; set; }
        public int Port { get; set; }
        public string Email { get; set;}
        public string Pwd { get; set;}
        public string Asunto { get; set; }
        public string Mensaje { get; set;}
        public string EmailDestino { get; set; }
    }
}
