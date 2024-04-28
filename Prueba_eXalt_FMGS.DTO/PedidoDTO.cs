using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Prueba_eXalt_FMGS.DTO
{
    public class CrearPedidoClienteDTO
    {
        public Guid? Id { get; set; }
        //public Guid UsuarioId { get; set; }
        public decimal ValorTotal { get { return Detalle.Sum(x => x.ValorTotal); } }
        public List<CrearPedidoClienteDetalleDTO> Detalle { get; set; } = new List<CrearPedidoClienteDetalleDTO>();
    }
    public class CrearPedidoClienteDetalleDTO
    {
        public Guid? Id { get; set; }
        public Guid ProductoId { get; set; }
        public string? Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal ValorPorUnidad { get; set; }
        public decimal ValorTotal { get { return ValorPorUnidad * Cantidad; } }
    }
}
