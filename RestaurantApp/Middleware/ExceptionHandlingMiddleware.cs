using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RestaurantApp.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    }
}
