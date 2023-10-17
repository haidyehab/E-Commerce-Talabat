using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.MidleWares
{
    public class ExceptionMiddlewares
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddlewares> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddlewares(RequestDelegate next , ILogger<ExceptionMiddlewares> logger ,IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);//log Exception in Database [production]
                                                 //in console

                //this for front
                context.Response.ContentType = "application/json"; //header
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;//500


                var response = _env.IsDevelopment()?
                    
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString());

                var options = new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                  await context.Response.WriteAsync(json);
            }
        }
    }
}
