using System;
using System.Collections.Generic;

namespace Prueba_eXalt_FMGS.DB.Entities
{
    public partial class Producto
    {
        public Producto()
        {
            PedidoDetalle = new HashSet<PedidoDetalle>();
            ProductoTipo = new HashSet<ProductoTipo>();
        }

        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int CantidadDisponible { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; }

        public virtual ICollection<PedidoDetalle> PedidoDetalle { get; set; }

        public virtual ICollection<ProductoTipo> ProductoTipo { get; set; }
    }
}
