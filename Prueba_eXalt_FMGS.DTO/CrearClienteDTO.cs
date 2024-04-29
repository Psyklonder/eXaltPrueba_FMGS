using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_eXalt_FMGS.DTO
{
    public class CrearClienteDTO
    {        
        //Persona
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public Guid LocalidadId { get; set; }
        //Usuario
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? AdminKey { get; set; }

    }
}
