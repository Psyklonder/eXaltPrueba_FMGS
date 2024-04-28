using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Prueba_eXalt_FMGS.API.InfraEstructura.Contratos;
using Prueba_eXalt_FMGS.API.InfraEstructura.Encriptacion;
using Prueba_eXalt_FMGS.API.InfraEstructura.Enum;
using Prueba_eXalt_FMGS.DB.Context;
using Prueba_eXalt_FMGS.DB.Entities;
using Prueba_eXalt_FMGS.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Prueba_eXalt_FMGS.API.InfraEstructura.Repositorios
{
    public class CuentaRepository : ICuentaRepository
    {

        private readonly eXaltDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public CuentaRepository(eXaltDbContext db, IMapper mapper, IConfiguration config)
        {
            _db = db;
            _mapper = mapper;
            _config = config;
        }

        public async Task<string> CrearCliente(CrearClienteDTO request)
        {
            if (await ExisteCorreo(request.Email))
            {
                return "Ya existe un usuario registrado con el correo electrónico suministrado.";
            }
            var RolCliente = await _db.Rol.Where(x => x.Codigo.ToUpper().Trim() == EnumRoles.CLIENTE.ToString()).FirstOrDefaultAsync();
            if (RolCliente == null)
            {
                return "No se puede crear el cliente. Por favor contacte al administrador del sistema.";
            }
            var localidad = await _db.Localidad.FindAsync(request.LocalidadId);
            if (localidad == null)
            {
                return "Su lugar de residencia no existe en la base de datos.";
            }

            var persona = _mapper.Map<Persona>(request);
            persona.Id = Guid.NewGuid();
            persona.Estado = true;
            var usuario = _mapper.Map<Usuario>(request);
            usuario.Id = Guid.NewGuid();
            usuario.PersonaId = persona.Id;
            usuario.RolId = RolCliente.Id;
            usuario.Password = EncriptarHASH256.Encriptar(request.Password);
            usuario.Estado = true;

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //INICIANDO TRANSACCIÓN PARA GUARDAR DATOS DE USUARIO
                    await _db.Persona.AddAsync(persona);
                    await _db.SaveChangesAsync();

                    await _db.Usuario.AddAsync(usuario);
                    await _db.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return "Cuenta creada.";
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        public async Task<LogInResponseDTO> InicioSesion(LogInDTO request)
        {
            var usuario = await ConsultarUsuario(request);
            if (usuario != null)
            {
                if (usuario.Estado)
                {
                    //GENERANDO TOKEN
                    var token = GenerarJWT(usuario);
                    return new LogInResponseDTO { Token = token, Nombre = $"{usuario.Persona.Nombres.Trim()} {usuario.Persona.Apellidos.Trim()}", Rol = usuario.Rol.Codigo };
                }
                else
                {
                    throw new Exception("El usuario se encuentra inhabilitado.");
                }
            }
            else
            {
                throw new Exception("Usuario o contraseña incorrectos.");
            }

        }

        async Task<bool> ExisteCorreo(string email)
        {
            if (await _db.Usuario.Where(x => x.Email.ToUpper().Trim() == email.ToUpper().Trim()).AsNoTracking().FirstOrDefaultAsync() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        async Task<Usuario?> ConsultarUsuario(LogInDTO userLogIn)
        {
            var response = await _db.Usuario.Include(x => x.Rol).Include(x => x.Persona).Where(x => x.Email == userLogIn.Email && x.Password == EncriptarHASH256.Encriptar(userLogIn.Password)).FirstOrDefaultAsync();
            if (response != null)
            {
                return response;
            }
            else
            {
                return null;
            }
        }
        string GenerarJWT(Usuario usuario)
        {
            var key = _config["Jwt:key"];
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            //Crear los claims
            var claims = new[] {
                new Claim("UsuarioId", usuario.Id.ToString()),
                new Claim("NombreCompleto", $"{usuario.Persona.Nombres} {usuario.Persona.Apellidos}"),
                new Claim("Email", usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol.Codigo)
            };
            //crear el token
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
