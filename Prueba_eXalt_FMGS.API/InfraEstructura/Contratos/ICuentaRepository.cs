using Microsoft.AspNetCore.Mvc;
using Prueba_eXalt_FMGS.DTO;

namespace Prueba_eXalt_FMGS.API.InfraEstructura.Contratos
{
    public interface ICuentaRepository
    {
        Task<string> CrearCliente(CrearClienteDTO request);

        Task<LogInResponseDTO> InicioSesion(LogInDTO request);
    }
}
