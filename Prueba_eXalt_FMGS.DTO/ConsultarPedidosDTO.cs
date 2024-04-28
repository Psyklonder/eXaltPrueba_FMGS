using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_eXalt_FMGS.DTO
{
    public class ConsultarPedidosDTO
    {
        public Guid PedidoId { get; set; }
        public string NombreUsuario { get; set; } = "";
        public DateTime Fecha { get; set; }
        public long NumeroFactura { get; set; }
        public string Estado { get; set; } = "";
        public decimal ValorTotal { get; set; }
        public int CantidadProductos { get; set; }
    }
}
