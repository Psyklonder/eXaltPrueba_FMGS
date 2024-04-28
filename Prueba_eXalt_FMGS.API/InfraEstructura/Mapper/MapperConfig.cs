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
                

            /*
            CreateMap<PedidoDetalle, CrearPedidoClienteDTO>()
                .ForMember(dest => dest.Detalle, opt => opt.MapFrom(src => src.de);
            */
            // CreateMap<Pedido, CrearPedidoClienteDTO>();
            /*   CreateMap<Producto, ConsultarProductoDTO>()          
                 .ForMember(dest => dest.Tipos, opt => opt.MapFrom(src => src.ProductoTipo.ToList()));

                 .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                 .ForMember(dest => dest.Tipos, opt => opt.MapFrom(src => src.ProductoTipo));
             */
        }
    }
}
