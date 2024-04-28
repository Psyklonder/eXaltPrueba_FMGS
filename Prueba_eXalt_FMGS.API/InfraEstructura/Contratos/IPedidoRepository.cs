using Prueba_eXalt_FMGS.DTO;
using System;
using System.Security.Claims;

namespace Prueba_eXalt_FMGS.API.InfraEstructura.Contratos
{
    public interface IPedidoRepository
    {
        Task<CrearPedidoClienteDTO> GuardarPedido(CrearPedidoClienteDTO request, ClaimsIdentity identity);
        Task<string> FinalizarPedido(Guid id, ClaimsIdentity identity);
        Task<string> CancelarPedido(Guid id, ClaimsIdentity identity);
        Task<string> BorrarProducto(BorrarProductoPedidoDTO request, ClaimsIdentity identity);
        Task<List<ConsultarPedidosDTO>> ConsultarPedidos();
        

    }
}
