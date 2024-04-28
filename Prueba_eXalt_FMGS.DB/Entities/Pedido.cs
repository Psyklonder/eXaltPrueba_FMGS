using System;
using System.Collections.Generic;

namespace Prueba_eXalt_FMGS.DB.Entities
{
    public partial class Pedido
    {
        public Pedido()
        {
            PedidoDetalle = new HashSet<PedidoDetalle>();
        }

        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public long NumeroFactura { get; set; }
        public bool Estado { get; set; }
        public decimal? Descuento { get; set; }
        public Guid PedidoEstadoId { get; set; }

        public virtual PedidoEstado PedidoEstado { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
        public virtual ICollection<PedidoDetalle> PedidoDetalle { get; set; }
    }
}
