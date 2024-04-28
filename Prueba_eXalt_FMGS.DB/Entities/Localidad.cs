using System;
using System.Collections.Generic;

namespace Prueba_eXalt_FMGS.DB.Entities
{
    public partial class Localidad
    {
        public Localidad()
        {
            InverseLocalidadPadre = new HashSet<Localidad>();
            Persona = new HashSet<Persona>();
        }

        public Guid Id { get; set; }
        public Guid? LocalidadPadreId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? CodigoPostal { get; set; }
        public bool Estado { get; set; }

        public virtual Localidad? LocalidadPadre { get; set; }
        public virtual ICollection<Localidad> InverseLocalidadPadre { get; set; }
        public virtual ICollection<Persona> Persona { get; set; }
    }
}
