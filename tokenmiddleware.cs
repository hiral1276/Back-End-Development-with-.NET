using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenValidationMiddleware> _logger;
    private const string AUTH_HEADER = "Authorization";
    private const string BEARER_PREFIX = "Bearer ";

    public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(AUTH_HEADER, out var authHeader) ||
            !authHeader.ToString().StartsWith(BEARER_PREFIX))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Missing or invalid token.");
            return;
        }

        var token = authHeader.ToString().Substring(BEARER_PREFIX.Length).Trim();

        // TODO: Replace this with your real token validation logic
        if (!IsValidToken(token))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid token.");
            return;
        }

        await _next(context);
    }

    // Dummy validation for demonstration; replace with real validation
    private bool IsValidToken(string token)
    {
        // Example: Accept only a specific token for demo
        return token == "your-valid-token";
    }
}
