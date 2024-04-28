using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_eXalt_FMGS.DTO
{
    public class EditarProductoDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public int CantidadDisponible { get; set; }
        public decimal Precio { get; set; }
        public bool Estado { get; set; }
    }
}
