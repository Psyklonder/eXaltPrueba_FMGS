using System;
using System.Collections.Generic;

namespace Prueba_eXalt_FMGS.DB.Entities
{
    public partial class Persona
    {
        public Persona()
        {
            Usuario = new HashSet<Usuario>();
        }

        public Guid Id { get; set; }
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public Guid LocalidadId { get; set; }
        public bool Estado { get; set; }

        public virtual Localidad Localidad { get; set; } = null!;
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
