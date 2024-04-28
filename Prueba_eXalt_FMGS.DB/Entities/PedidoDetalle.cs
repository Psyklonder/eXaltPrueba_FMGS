using System;
using System.Collections.Generic;

namespace Prueba_eXalt_FMGS.DB.Entities
{
    public partial class PedidoDetalle
    {
        public Guid Id { get; set; }
        public Guid ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal ValorPorUnidad { get; set; }
        public Guid PedidoId { get; set; }

        public virtual Pedido Pedido { get; set; } = null!;
        public virtual Producto Producto { get; set; } = null!;
    }
}
