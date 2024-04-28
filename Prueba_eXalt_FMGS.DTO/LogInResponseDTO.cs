using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_eXalt_FMGS.DTO
{
    public class LogInResponseDTO
    {        
        public string Token { get; set; } = "";
        //SE DEJA EL ROL EXPLÍCITO EN LA RESPUESTA POR PROPÓSITO (POSIBLE) DE VALIDACIONES EN EL FRONT
        public string Rol { get; set; } = "";
        public string Nombre { get; set; } = "";
    }
}
