using System.Net;

namespace NetKubernetes.Middlware;

public class MiddlwareException :Exception
{
    public HttpStatusCode Codigo {get ;set;}
    public object? Errores {get;set;}

    public MiddlwareException(HttpStatusCode codigo, object? errores = null)
    {
        Codigo = codigo;
        Errores = errores;
    }
}