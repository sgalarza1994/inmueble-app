using NetKubernetes.Dtos.UsuariosDto;
namespace NetKubernetes.Data.Usuarios;

public interface IUsuarioRepositoryy 
{
    Task<UsuarioResponseDto> GetUsuario();
    Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto request);
    Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRequestDto request);
}