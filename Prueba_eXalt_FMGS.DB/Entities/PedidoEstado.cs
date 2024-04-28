using System;
using System.Collections.Generic;

namespace Prueba_eXalt_FMGS.DB.Entities
{
    public partial class PedidoEstado
    {
        public PedidoEstado()
        {
            Pedido = new HashSet<Pedido>();
        }

        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }

        public virtual ICollection<Pedido> Pedido { get; set; }
    }
}
