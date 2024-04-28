using System;
using System.Collections.Generic;

namespace Prueba_eXalt_FMGS.DB.Entities
{
    public partial class ProductoTipo
    {
        public ProductoTipo()
        {
            Producto = new HashSet<Producto>();
        }

        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }

        public virtual ICollection<Producto> Producto { get; set; }
    }
}
