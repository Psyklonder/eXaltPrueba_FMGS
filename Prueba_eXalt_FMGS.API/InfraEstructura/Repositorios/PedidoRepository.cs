using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Prueba_eXalt_FMGS.API.InfraEstructura.Contratos;
using Prueba_eXalt_FMGS.API.InfraEstructura.Enum;
using Prueba_eXalt_FMGS.API.InfraEstructura.EnvioCorreo;
using Prueba_eXalt_FMGS.DB.Context;
using Prueba_eXalt_FMGS.DB.Entities;
using Prueba_eXalt_FMGS.DTO;
using System.Collections.Generic;
using System.Data;

namespace Prueba_eXalt_FMGS.API.InfraEstructura.Repositorios
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly eXaltDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public PedidoRepository(eXaltDbContext db, IMapper mapper, IConfiguration config)
        {
            _db = db;
            _mapper = mapper;
            _config = config;
        }

        public async Task<string> CancelarPedido(Guid id)
        {
            var pedido = await ExistePedido(id);
            var estado = await _db.PedidoEstado.Where(x => x.Nombre.ToUpper().Trim() == EnumPedidoEstados.CANCELADO.ToString().ToUpper().Trim()).FirstOrDefaultAsync();
            if (pedido == null)
            {
                return "No existe el pedido.";
            }
            else if (pedido.PedidoEstadoId.Equals(estado.Id))
            {
                return "El pedido ya fue cancelado.";
            }
            else
            {
                try
                {
                    pedido.PedidoEstadoId = estado.Id;
                    //EVITAR CONFLICTO AL ACTUALIZAR COLUMNA IDENTITY EN LA BASE DE DATOS
                    _db.Entry(pedido).Property(x => x.NumeroFactura).IsModified = false;
                    await _db.SaveChangesAsync();
                    await RetornarStockProductos(id);
                    return "Pedido cancelado.";
                }
                catch (Exception e)
                {
                    throw;
                }

            }
            throw new NotImplementedException();
        }

        public async Task<string> FinalizarPedido(Guid id)
        {
            var pedido = await ExistePedido(id);
            var estado = await _db.PedidoEstado.AsNoTracking().Where(x => x.Nombre.ToUpper().Trim() == EnumPedidoEstados.FACTURADO.ToString().ToUpper().Trim()).FirstOrDefaultAsync();
            if (pedido == null)
            {
                return "No existe el pedido.";
            }
            else if (pedido.PedidoEstadoId.Equals(estado.Id))
            {
                return "El pedido ya fue facturado.";
            }
            else
            {
                try
                {
                    pedido.PedidoEstadoId = estado.Id;
                    //EVITAR CONFLICTO AL ACTUALIZAR COLUMNA IDENTITY EN LA BASE DE DATOS
                    _db.Entry(pedido).Property(x => x.NumeroFactura).IsModified = false;
                    await _db.SaveChangesAsync();
                    EnviarCorreoConfirmacion(pedido);
                    return "Compra realizada.";
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public async Task<string> BorrarProducto(BorrarProductoPedidoDTO request)
        {
            var pedido = await ExistePedido(request.PedidoId);
            var estado = await _db.PedidoEstado.AsNoTracking().Where(x => x.Nombre.ToUpper().Trim() == EnumPedidoEstados.PENDIENTE.ToString().ToUpper().Trim()).FirstOrDefaultAsync();
            //VALIDACIONES
            if (pedido == null)
            {
                return "No existe el pedido.";
            }
            if (!pedido.PedidoEstadoId.Equals(estado.Id))
            {
                return "No es posible modificar el pedido.";
            }
            if (pedido.PedidoDetalle.Where(x => x.ProductoId.Equals(request.ProductoId)).FirstOrDefault() == null)
            {
                return "El producto no ha sido agregado al pedido.";
            }

            try
            {
                var registro = await _db.PedidoDetalle.Include(x => x.Producto).AsNoTracking().Where(x => x.PedidoId.Equals(pedido.Id) && x.ProductoId.Equals(request.ProductoId)).FirstOrDefaultAsync();
                await RetornarStockProductoDetalle(registro);

                _db.PedidoDetalle.Remove(registro);
                await _db.SaveChangesAsync();

                return "Compra realizada.";
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<CrearPedidoClienteDTO> GuardarPedido(CrearPedidoClienteDTO request)
        {
            //VALIDANDO SI EXISTE EL CLIENTE
            if (!await ExisteCliente(request.UsuarioId))
            {
                throw new Exception("El cliente suministrado no existe.");
            }
            //VERIFICAR SI ES NUEVO PEDIDO O SI SE VAN A AGREGAR A UNO EXISTENTE
            if (String.IsNullOrEmpty(request.Id.ToString()))
            {
                return await NuevoPedido(request);
            }
            else
            {
                throw new Exception("Actualizar pedido no implementado.");
            }
        }

        #region Metodos privados
        async Task<CrearPedidoClienteDTO> NuevoPedido(CrearPedidoClienteDTO request)
        {
            //VALIDANDO SI EXISTE EL CLIENTE
            if (!await ExisteCliente(request.UsuarioId))
            {
                throw new Exception("El cliente suministrado no existe.");
            }
            //VALIDANDO SI EXISTEN LOS PRODUCTOS AGREGADOS
            var producto = new Producto();
            var productos = new List<Producto>();
            foreach (var item in request.Detalle)
            {
                producto = await ExisteProducto(item.ProductoId);
                if (producto == null)
                {
                    throw new Exception("Uno de los productos no existe o no se encuentra disponible.");
                }
                else if (producto.CantidadDisponible - item.Cantidad < 0)
                {
                    throw new Exception($"No existen suficientes cantidades disponibles para el producto: {producto.Nombre}.");
                }
                //item.ValorPorUnidad = producto.Precio;
                productos.Add(producto);
            }

            var registro = _mapper.Map<Pedido>(request);
            //ASIGNANDO DATOS ADICIONALES DEL PEDIDO
            registro.Estado = true;
            registro.PedidoEstadoId = _db.PedidoEstado.Where(x => x.Nombre.ToUpper() == EnumPedidoEstados.PENDIENTE.ToString()).FirstOrDefault().Id;

            var detalle = _mapper.Map<List<PedidoDetalle>>(request.Detalle);
            detalle.ForEach(x => x.ValorPorUnidad = productos.Where(y => y.Id.Equals(x.ProductoId)).FirstOrDefault().Precio);
            //ASIGNANDO EL ID DEL PEDIDO AL DETALLE
            detalle.ForEach(x => x.PedidoId = registro.Id);
            //INICIANDO TRANSACCIÓN
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //DESCONTANDO CANTIDADES A PRODUCTOS 
                    await DescontarStockProductos(detalle);
                    await _db.Pedido.AddAsync(registro);
                    await _db.SaveChangesAsync();
                    await _db.PedidoDetalle.AddRangeAsync(detalle);
                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            var response = _mapper.Map<CrearPedidoClienteDTO>(registro);
            response.Detalle = _mapper.Map<List<CrearPedidoClienteDetalleDTO>>(detalle);
            response.Detalle.ForEach(x => x.Nombre = productos.Where(y => y.Id.Equals(x.ProductoId)).FirstOrDefault()?.Nombre);
            //response.Detalle.ForEach(x => x.ValorPorUnidad = productos.Where(y => y.Id.Equals(x.ProductoId)).FirstOrDefault().Precio);
            return response;
        }
        async Task<bool> ExisteCliente(Guid id)
        {
            return await _db.Usuario.Where(x => x.Id.Equals(id) && x.Estado).FirstOrDefaultAsync() == null ? false : true;
        }
        async Task<Producto?> ExisteProducto(Guid id)
        {
            return await _db.Producto.Where(x => x.Id.Equals(id) && x.Estado).FirstOrDefaultAsync();
        }
        async Task<Pedido> ExistePedido(Guid id)
        {
            return await _db.Pedido.Include(x => x.Usuario).Include(x => x.PedidoDetalle).ThenInclude(y => y.Producto).Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
        }
        async Task DescontarStockProductos(List<PedidoDetalle> request)
        {
            foreach (var item in request)
            {
                var producto = await _db.Producto.FindAsync(item.ProductoId);
                producto.CantidadDisponible -= item.Cantidad;
                _db.Entry(producto).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
        }
        async Task RetornarStockProductos(Guid pedidoId)
        {
            var pedido = await _db.Pedido.Include(x => x.PedidoDetalle).ThenInclude(y => y.Producto).Where(x => x.Id.Equals(pedidoId)).FirstOrDefaultAsync();

            foreach (var item in pedido.PedidoDetalle)
            {
                var producto = await _db.Producto.FindAsync(item.ProductoId);
                producto.CantidadDisponible += item.Cantidad;
                _db.Entry(producto).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
        }
        async Task RetornarStockProductoDetalle(PedidoDetalle request)
        {
            request.Producto.CantidadDisponible += request.Cantidad;
            _db.Entry(request.Producto).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
        void EnviarCorreoConfirmacion(Pedido request)
        {
            //GENERANDO MENSAJE DE ENVÍO AL CORREO
            string mensaje = $@"<table style=""width:100%""><thead>
                                  <tr >
                                    <td colspan=""3"" style=""text-align:left""><b>Factura No.</b> {request.NumeroFactura}</td>    
                                  </tr>
                                  <tr>
                                    <th style=""text-align:left"">Producto</th>
                                    <th style=""text-align:left"">Cantidad</th>
                                    <th style=""text-align:left"">Valor</th>
                                  </tr>
                                </thead>";
            mensaje += "<tbody>";
            foreach (var item in request.PedidoDetalle)
            {
                mensaje += $@"<tr>
                                <td>{item.Producto.Nombre}</td>
                                <td>{item.Cantidad}</td>
                                <td>{(item.ValorPorUnidad * item.Cantidad).ToString("C0")}</td>
                             </tr>";
            }
            mensaje += "</tbody><tfoot>";
            mensaje += $@"<tr >
                           <td colspan=""2""></td>    
                            <td colspan=""1"" style=""text-align:left""><b>Total:</b> {request.PedidoDetalle.Sum(x => x.ValorPorUnidad * x.Cantidad).ToString("C0")}</td>    
                          </tr>";
            mensaje += "</tfoot></table>";

            MailConfirmarPedido enviarCorreo = new MailConfirmarPedido
            {
                SmtpClient = _config["MailConfig:SmtpClient"].ToString(),
                Port = int.Parse(_config["MailConfig:Port"].ToString()),
                Email = _config["MailConfig:Email"].ToString(),
                Pwd = _config["MailConfig:Pwd"].ToString(),
                Asunto = $"Pedido No. {request.NumeroFactura} confirmado.",
                Mensaje = mensaje,
                EmailDestino = request.Usuario.Email
            };
            EnviarCorreo.ConfirmarPedido(enviarCorreo);
        }
        #endregion
    }
}
