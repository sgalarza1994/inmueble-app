using System.Net;
using Newtonsoft.Json;

namespace NetKubernetes.Middlware;


public class ManagerMiddlware 
{
    private readonly     RequestDelegate _next;
    private readonly ILogger<ManagerMiddlware> _logger;

    public ManagerMiddlware(RequestDelegate next, ILogger<ManagerMiddlware> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task Invoke(HttpContext context)
    {
        try 
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            await MangerExceptionAsync(context,ex,_logger);
        }
    }


    private async Task MangerExceptionAsync(HttpContext context, Exception ex , ILogger<ManagerMiddlware> logger)
    {
        object? errores = null;

        switch(ex)
        {
            case MiddlwareException me :
                logger.LogError(ex, "ManagerMiddlware Error");
                errores = me.Errores;
                context.Response.StatusCode = (int)me.Codigo;
                break;

            case Exception e : 
                logger.LogError(ex, "Exception Error");
                 errores = string.IsNullOrWhiteSpace(e.Message)  ? "Error" : e.Message;
                 context.Response.StatusCode     = (int)HttpStatusCode.InternalServerError;
            break;
        }

      context.Response.ContentType = "application/json";
      var resultados = string.Empty;
      if(errores != null)
      {
          resultados = JsonConvert.SerializeObject(new {errores});
      }
      await context.Response.WriteAsync(resultados);
    }
}