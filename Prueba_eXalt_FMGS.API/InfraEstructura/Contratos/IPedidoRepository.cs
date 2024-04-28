using Prueba_eXalt_FMGS.DTO;
using System;

namespace Prueba_eXalt_FMGS.API.InfraEstructura.Contratos
{
    public interface IPedidoRepository
    {
        Task<CrearPedidoClienteDTO> GuardarPedido(CrearPedidoClienteDTO request);
        Task<string> FinalizarPedido(Guid id);
        Task<string> CancelarPedido(Guid id);
        Task<string> BorrarProducto(BorrarProductoPedidoDTO request);

    }
}
