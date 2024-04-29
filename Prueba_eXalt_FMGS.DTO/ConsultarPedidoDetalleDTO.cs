using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_eXalt_FMGS.DTO
{
    public class ConsultarPedidoDetalleDTO
    {
        public string NombreProducto { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal ValorPorUnidad { get; set; }
        public decimal ValorTotal { get { return ValorPorUnidad * Cantidad; } }
    }
}
