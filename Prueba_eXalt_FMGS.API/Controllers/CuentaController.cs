using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Prueba_eXalt_FMGS.API.InfraEstructura.Contratos;
using Prueba_eXalt_FMGS.DB.Entities;
using Prueba_eXalt_FMGS.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Prueba_eXalt_FMGS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaRepository _repository;
        public CuentaController(ICuentaRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] CrearClienteDTO request)
        {
            try
            {
                return Ok(await _repository.CrearCliente(request));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InicioSesion([FromBody] LogInDTO request)
        {
            try
            {
                return Ok(await _repository.InicioSesion(request));
            }
            catch (Exception error)
            {
                return NotFound(error.Message);
            }
        }
    }
}
