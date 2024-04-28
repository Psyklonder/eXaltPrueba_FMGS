using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prueba_eXalt_FMGS.API.InfraEstructura.Contratos;
using Prueba_eXalt_FMGS.API.InfraEstructura.Enum;
using Prueba_eXalt_FMGS.DB.Context;
using Prueba_eXalt_FMGS.DB.Entities;
using Prueba_eXalt_FMGS.DTO;

namespace Prueba_eXalt_FMGS.API.InfraEstructura.Repositorios
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly eXaltDbContext _db;
        private readonly IMapper _mapper;
        public ProductoRepository(eXaltDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<ConsultarProductoDTO>> ConsultarProductos(bool esAdmin)
        {
            var response = new List<ConsultarProductoDTO>();
            try
            {         
                if (esAdmin)
                {
                    response = _mapper.Map<List<ConsultarProductoDTO>>(await _db.Producto.Include(x => x.ProductoTipo).OrderBy(x => x.Nombre).ToListAsync());
                }
                else 
                {
                    response = _mapper.Map<List<ConsultarProductoDTO>>(await _db.Producto.Include(x => x.ProductoTipo).Where(x=>x.Estado).OrderBy(x => x.Nombre).ToListAsync());
                }
                
            }
            catch (Exception e)
            {
                throw;
            }
            return response;
        }

        public async Task<ConsultarProductoDTO> ConsultarProducto(Guid id, bool esAdmin)
        {
            var response = new ConsultarProductoDTO();
            try
            {
                if (esAdmin)
                {
                    response = _mapper.Map<ConsultarProductoDTO>(await _db.Producto.Include(x => x.ProductoTipo).Where(x => x.Id.Equals(id)).FirstOrDefaultAsync());
                }
                else
                {
                    response = _mapper.Map<ConsultarProductoDTO>(await _db.Producto.Include(x => x.ProductoTipo).Where(x => x.Id.Equals(id) && x.Estado).FirstOrDefaultAsync());
                }
                
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<string> CrearProducto(CrearProductoDTO request)
        {
            try
            {
                var registro = _mapper.Map<Producto>(request);
                await _db.Producto.AddAsync(registro);
                await _db.SaveChangesAsync();
                return "Producto creado.";
            }
            catch (Exception)
            {

                throw;
            }            
        }

        public async Task<EditarProductoDTO?> EditarProducto(EditarProductoDTO request)
        {
            try
            {
                var registro = await _db.Producto.Where(x => x.Id.Equals(request.Id)).AsNoTracking().FirstOrDefaultAsync();
                if (registro != null)
                {
                    registro = _mapper.Map<Producto>(request);
                    _db.Entry(registro).State = EntityState.Modified; 
                    await _db.SaveChangesAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return request;
        }

        public async Task<string?> BorrarProducto(Guid id)
        {
            try
            {
                var registro = await _db.Producto.Where(x => x.Id.Equals(id)).AsNoTracking().FirstOrDefaultAsync();
                if (registro != null)
                {
                    registro.Estado = false;
                    _db.Entry(registro).State = EntityState.Modified; 
                    await _db.SaveChangesAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return "Producto borrado.";
        }

    }
}
