using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        // Hàm xử lý HTTP request
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(
            ILogger<ExceptionHandlerMiddleware> logger,
            RequestDelegate next)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                var errorId = Guid.NewGuid();

                // Log this exception
                _logger.LogError(e, $"{errorId} : {e.Message}");

                // Return a custom error exception
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong!"
                };

                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
