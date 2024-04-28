using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_eXalt_FMGS.DTO
{
    public class BorrarProductoPedidoDTO
    {
        public Guid PedidoId { get; set; }
        public Guid ProductoId { get; set; }
    }
}
