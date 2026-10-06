using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ProjectEnergy.Web.Infrastructure;

/// <summary>
/// Protege a pré-visualização partilhada por túnel (ex.: Cloudflare Tunnel) antes de existir alojamento.
/// Pedidos externos — que chegam via Cloudflare (cabeçalho Cf-Ray) ou com um Host que não é local —
/// nunca acedem ao backoffice e só veem o site com a palavra-passe definida em "Preview:Password".
/// Sem palavra-passe configurada, todo o acesso externo é recusado.
/// </summary>
public class PreviewAccessMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<PreviewAccessMiddleware> logger)
{
    private const string Realm = "Project Energy - pre-visualizacao";

    public async Task InvokeAsync(HttpContext context)
    {
        if (!IsExternal(context.Request))
        {
            await next(context);
            return;
        }

        if (context.Request.Path.StartsWithSegments("/umbraco", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        var password = configuration["Preview:Password"];
        if (string.IsNullOrEmpty(password))
        {
            logger.LogWarning("Pedido externo recusado: Preview:Password não está configurada.");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        if (!HasValidPassword(context.Request, password))
        {
            context.Response.Headers.WWWAuthenticate = $"Basic realm=\"{Realm}\", charset=\"UTF-8\"";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        context.Response.Headers["X-Robots-Tag"] = "noindex, nofollow";
        await next(context);
    }

    private static bool IsExternal(HttpRequest request)
    {
        if (request.Headers.ContainsKey("Cf-Ray"))
        {
            return true;
        }

        var host = request.Host.Host;
        return !(string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase)
                 || (IPAddress.TryParse(host.Trim('[', ']'), out var ip) && IPAddress.IsLoopback(ip)));
    }

    // Autenticação básica: o nome de utilizador é ignorado, só a palavra-passe conta.
    private static bool HasValidPassword(HttpRequest request, string expected)
    {
        var header = request.Headers.Authorization.ToString();
        if (!header.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        string decoded;
        try
        {
            decoded = Encoding.UTF8.GetString(Convert.FromBase64String(header["Basic ".Length..].Trim()));
        }
        catch (FormatException)
        {
            return false;
        }

        var supplied = decoded[(decoded.IndexOf(':') + 1)..];
        return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(supplied), Encoding.UTF8.GetBytes(expected));
    }
}
