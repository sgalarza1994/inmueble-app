using System.Security.Claims;

namespace  NetKubernetes.Token;

public class UsuarioSesion : IUsuarioSesion

{
    private readonly IHttpContextAccessor _httpContextAccesor;
    public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccesor = httpContextAccessor;
    }
    public string ObtenerUsuarioSesion()
    {
        var unserName = _httpContextAccesor.HttpContext!.User?.Claims?.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier)?.Value;
        return unserName!;
    }
}