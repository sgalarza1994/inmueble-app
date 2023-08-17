using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Usuarios;
using NetKubernetes.Dtos.UsuariosDto;

namespace NetKubernetes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController: ControllerBase 
{
    private readonly IUsuarioRepositoryy _usuarioRepository;


    public UsuarioController(IUsuarioRepositoryy usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UsuarioResponseDto>> Login([FromBody]UsuarioLoginRequestDto request)
    {
        return await _usuarioRepository.Login(request);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UsuarioResponseDto>> Registrar([FromBody]UsuarioRegistroRequestDto request)
    {
        return await _usuarioRepository.RegistroUsuario(request);
    }
    [Authorize]
    [HttpPost("obtenerUsuario")]
    public async Task<ActionResult<UsuarioResponseDto>> ObtenerUsuario()
    {
        return await _usuarioRepository.GetUsuario();
    }


}