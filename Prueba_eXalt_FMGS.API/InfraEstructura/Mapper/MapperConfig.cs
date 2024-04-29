namespace Prueba_eXalt_FMGS.API.InfraEstructura.Mapper
{
    using AutoMapper;
    using Prueba_eXalt_FMGS.DB.Entities;
    using Prueba_eXalt_FMGS.DTO;

    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Producto, ConsultarProductoDTO>().ForMember(dest => dest.Tipos, opt => opt.MapFrom(src => src.ProductoTipo.ToList()));
            CreateMap<ProductoTipo, TipoProductoDTO>();
            CreateMap<EditarProductoDTO, Producto>();
            CreateMap<Producto, CrearProductoDTO>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true));

            CreateMap<CrearClienteDTO, Persona>()
                .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombres))
                .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Apellidos))
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.LocalidadId, opt => opt.MapFrom(src => src.LocalidadId));

            CreateMap<CrearClienteDTO, Usuario>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<Pedido, CrearPedidoClienteDTO>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (String.IsNullOrEmpty(src.Id.ToString())) ? Guid.NewGuid() : src.Id))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<PedidoDetalle, CrearPedidoClienteDetalleDTO>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (String.IsNullOrEmpty(src.Id.ToString())) ? Guid.NewGuid() : src.Id));

            CreateMap<Pedido, ConsultarPedidosDTO>()
                .ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Persona.Nombres + " " + src.Usuario.Persona.Apellidos))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.NumeroFactura, opt => opt.MapFrom(src => src.NumeroFactura))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.PedidoEstado.Nombre))
                .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.PedidoDetalle.Sum(x => x.Cantidad * x.ValorPorUnidad)))
                .ForMember(dest => dest.CantidadReferencias, opt => opt.MapFrom(src => src.PedidoDetalle.Count));

            CreateMap<PedidoDetalle, ConsultarPedidoDetalleDTO>()
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto.Nombre))
                .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Cantidad))
                .ForMember(dest => dest.ValorPorUnidad, opt => opt.MapFrom(src => src.ValorPorUnidad));
         
        }
    }
}
