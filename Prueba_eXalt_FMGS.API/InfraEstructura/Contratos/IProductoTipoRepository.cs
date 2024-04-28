using Prueba_eXalt_FMGS.DTO;

namespace Prueba_eXalt_FMGS.API.InfraEstructura.Contratos
{
    public interface IProductoTipoRepository
    {        
        Task<TipoProductoDTO> CrearTipo(TipoProductoDTO request);
    }
}
