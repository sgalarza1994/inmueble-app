using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Dtos.UsuariosDto;
using NetKubernetes.Middlware;
using NetKubernetes.Models;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Usuarios;

public class UsuarioRepository : IUsuarioRepositoryy
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _SignManager;

    private readonly IJwtGenerador _jwtGeneratorr;
    private readonly AppDbContext _context;
    private readonly IUsuarioSesion _usuarioRepositoy;


    public UsuarioRepository(UserManager<Usuario> userManager, SignInManager<Usuario> SignManager,IJwtGenerador jwtGenerador, AppDbContext context, 
            IUsuarioSesion usuarioRepository
    )
    {
        _userManager= userManager;
        _SignManager     = SignManager;
        _jwtGeneratorr = jwtGenerador;
        _context = context;
        _usuarioRepositoy = usuarioRepository;


    }


    public async Task<UsuarioResponseDto> GetUsuario()
    {
        var usuario =  await _userManager.FindByNameAsync(_usuarioRepositoy.ObtenerUsuarioSesion());
        if(usuario is null)
        {
            throw new MiddlwareException(System.Net.HttpStatusCode.Unauthorized,new {mensaje = "Usuario del token no existe en la base de datos"});
        }
        var usuarioDto = TransformerToUserDto(usuario!);
        return usuarioDto;

    }

    public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto request)
    {
        var usuario = await _userManager.FindByEmailAsync(request.Email!);
        if(usuario is null)
        {
            throw new MiddlwareException(System.Net.HttpStatusCode.Unauthorized,new {mensaje = "El Email del usuario no existe en mi base de datos"});
        }
        var resultado = await _SignManager.CheckPasswordSignInAsync(usuario!,request.Password!,false);
        if(resultado.Succeeded)
        {
             return TransformerToUserDto(usuario!);
        }
        throw new MiddlwareException(System.Net.HttpStatusCode.Unauthorized,new {mensaje = "Las credenciales son incorrectas"});
    }

    public async Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRequestDto request)
    {
        var existeEmail = await _context.Users.Where(x=>x.Email == request.Email).AnyAsync();
         if(existeEmail)
         {
            throw new MiddlwareException(System.Net.HttpStatusCode.BadRequest,new {mensaje = "Email ya se encuentra registrado"});
         }
        existeEmail = await _context.Users.Where(x=>x.UserName == request.UserName).AnyAsync();
         if(existeEmail)
         {
            throw new MiddlwareException(System.Net.HttpStatusCode.BadRequest,new {mensaje = "userName ya existe"});
         }


        var usuario = new Usuario 
        { 
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Email = request.Email,
            UserName = request.UserName,
            Telefono = request.Telefono
        };
        var response = await _userManager.CreateAsync(usuario!,request.Password!);
        if(response.Succeeded)
            return TransformerToUserDto(usuario);
        throw new MiddlwareException(System.Net.HttpStatusCode.BadRequest,new {mensaje = "No se pudo crear la exception"});
    }


    private UsuarioResponseDto TransformerToUserDto(Usuario usuario)
    {
        return new UsuarioResponseDto 
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Telefono = usuario.Telefono,
            Email = usuario.Email,
            UserName = usuario.UserName,
            Token = _jwtGeneratorr.CrearToken(usuario)
        };
    }
}