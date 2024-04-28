using System;
using System.Collections.Generic;

namespace Prueba_eXalt_FMGS.DB.Entities
{
    public partial class Usuario
    {
        public Usuario()
        {
            Pedido = new HashSet<Pedido>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid RolId { get; set; }
        public Guid PersonaId { get; set; }
        public bool Estado { get; set; }

        public virtual Persona Persona { get; set; } = null!;
        public virtual Rol Rol { get; set; } = null!;
        public virtual ICollection<Pedido> Pedido { get; set; }
    }
}
