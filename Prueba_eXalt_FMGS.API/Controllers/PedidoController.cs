using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prueba_eXalt_FMGS.API.InfraEstructura.Contratos;
using Prueba_eXalt_FMGS.API.InfraEstructura.Encriptacion;
using Prueba_eXalt_FMGS.DTO;
using System.Net;
using System.Net.Mail;

namespace Prueba_eXalt_FMGS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _repository;
        //private readonly IConfiguration _config;
        public PedidoController(IPedidoRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,CLIENTE")]
        public async Task<IActionResult> GuardarPedido([FromBody] CrearPedidoClienteDTO request)
        {
            try
            {
                return Ok(await _repository.GuardarPedido(request));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,CLIENTE")]
        public async Task<IActionResult> FinalizarPedido(Guid id)
        {
            try
            {
                return Ok(await _repository.FinalizarPedido(id));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN,CLIENTE")]
        public async Task<IActionResult> BorrarProductoPedido(BorrarProductoPedidoDTO request)
        {
            try
            {
                return Ok(await _repository.BorrarProducto(request));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN,CLIENTE")]
        public async Task<IActionResult> CancelarPedido(Guid id)
        {
            try
            {

                return Ok(await _repository.CancelarPedido(id));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


    }
}
