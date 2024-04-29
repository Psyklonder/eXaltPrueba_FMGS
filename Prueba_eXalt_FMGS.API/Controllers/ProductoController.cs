using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prueba_eXalt_FMGS.API.InfraEstructura.Contratos;
using Prueba_eXalt_FMGS.API.InfraEstructura.Enum;
using Prueba_eXalt_FMGS.DB.Entities;
using Prueba_eXalt_FMGS.DTO;
using System.Security.Claims;

namespace Prueba_eXalt_FMGS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepository _repository;
        public ProductoController(IProductoRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        [Authorize(Roles = "ADMIN,CLIENTE")]
        public async Task<IActionResult> ConsultarProductos()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value.Trim() == EnumRoles.ADMIN.ToString())
                {
                    //SE RETORNAN TODOS LOS PRODUCTOS SIN IMPORTAR ESTEN ACTIVOS O NO
                    return Ok(await _repository.ConsultarProductos(true));
                }
                else
                {
                    return Ok(await _repository.ConsultarProductos(false));
                }

            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN,CLIENTE")]
        public async Task<IActionResult> ConsultarProducto(Guid id)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value.Trim() == EnumRoles.ADMIN.ToString())
                {
                    //SE RETORNAN EL PRODUCTO SIN IMPORTAR ESTÉ ACTIVO O NO
                    return Ok(await _repository.ConsultarProducto(id, true));
                }
                else
                {
                    return Ok(await _repository.ConsultarProducto(id, false));
                }

            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CrearProducto([FromBody] CrearProductoDTO request)
        {
            try
            {
                return Ok(await _repository.CrearProducto(request));
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> EditarProducto([FromBody] EditarProductoDTO request)
        {
            try
            {
                var response = await _repository.EditarProducto(request);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound("Producto no encontrado.");
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> BorrarProducto(Guid id)
        {
            try
            {
                var response = await _repository.BorrarProducto(id);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound("Producto no encontrado.");
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


    }
}
