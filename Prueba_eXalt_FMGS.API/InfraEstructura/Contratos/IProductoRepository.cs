using Microsoft.AspNetCore.Mvc;
using Prueba_eXalt_FMGS.DTO;

namespace Prueba_eXalt_FMGS.API.InfraEstructura.Contratos
{
    public interface IProductoRepository
    {
        Task<List<ConsultarProductoDTO>> ConsultarProductos(bool esAdmin);
        Task<ConsultarProductoDTO> ConsultarProducto(Guid id, bool esAdmin);
        Task<string> CrearProducto(CrearProductoDTO request);
        Task<EditarProductoDTO?> EditarProducto(EditarProductoDTO request);
        Task<string?> BorrarProducto(Guid id);
    }
}
