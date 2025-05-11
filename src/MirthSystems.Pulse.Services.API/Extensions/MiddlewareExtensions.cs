namespace MirthSystems.Pulse.Services.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }

        public static IApplicationBuilder UseSecureHeaders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecureHeadersMiddleware>();
        }
    }

    class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response is HttpResponse response && response.StatusCode == 404)
                {
                    await response.WriteAsJsonAsync(new
                    {
                        message = "Not Found"
                    });
                }
                else if (context.Response is HttpResponse forbiddenResponse && forbiddenResponse.StatusCode == 403)
                {
                    await forbiddenResponse.WriteAsJsonAsync(new
                    {
                        error = "insufficient_permissions",
                        error_description = "Insufficient permissions to access resource",
                        message = "Permission denied"
                    });
                }
                else if (context.Response is HttpResponse unauthorizedResponse && unauthorizedResponse.StatusCode == 401)
                {
                    await unauthorizedResponse.WriteAsJsonAsync(
                        new
                        {
                            message = context.Request.Headers.ContainsKey("Authorization")
                                            ? "Bad credentials"
                                            : "Requires authentication"
                        });
                }
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                message = "Internal Server Error."
            });
        }
    }

    class SecureHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecureHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Append("X-XSS-Protection", "0");
            context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
            context.Response.Headers.Append("X-Frame-Options", "deny");
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; frame-ancestors 'none';");
            context.Response.Headers.Append("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
            context.Response.Headers.Append("Pragma", "no-cache");

            await _next(context);
        }
    }
}
