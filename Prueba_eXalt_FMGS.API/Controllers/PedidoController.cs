using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prueba_eXalt_FMGS.API.InfraEstructura.Contratos;
using Prueba_eXalt_FMGS.API.InfraEstructura.Encriptacion;
using Prueba_eXalt_FMGS.DTO;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

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

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ConsultarPedidos()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                return Ok(await _repository.ConsultarPedidos());
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ConsultarPedidoDetalle(Guid id)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                return Ok(await _repository.ConsultarPedidoDetalle(id));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,CLIENTE")]
        public async Task<IActionResult> GuardarPedido([FromBody] CrearPedidoClienteDTO request)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                return Ok(await _repository.GuardarPedido(request, identity));
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
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                return Ok(await _repository.FinalizarPedido(id, identity));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
        /*
         IMPLEMENTACIÓN NO NECESARIA
        [HttpDelete]
        [Authorize(Roles = "ADMIN,CLIENTE")]
        public async Task<IActionResult> BorrarProductoPedido(BorrarProductoPedidoDTO request)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                return Ok(await _repository.BorrarProducto(request, identity));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
        */
        [HttpPut]
        [Authorize(Roles = "ADMIN,CLIENTE")]
        public async Task<IActionResult> CancelarPedido(Guid id)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                return Ok(await _repository.CancelarPedido(id, identity));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


    }
}
