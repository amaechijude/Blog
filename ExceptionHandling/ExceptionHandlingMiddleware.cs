using System.Net;
using System.Text.Json;
using Blog.Repository;

namespace Blog.ExceptionHandling
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        public const string httpContentType = "application/json";
        public async Task InvokeAsync(HttpContext context)
        {
            try 
            {
                await _next(context);
                return;
            }
            catch (KeyNotFoundException ex)
            {
                context.Response.ContentType = httpContentType;
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                var errorResponse = new 
                {
                    code = (int)HttpStatusCode.NotFound,
                    status = "failed",
                    message = $"{ex.Message}"
                };

                var jsonRespose = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonRespose);
                
                return;
            }
            catch (LikedException ex)
            {
                context.Response.ContentType = httpContentType;
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                var errorResponse = new 
                {
                    code = (int)HttpStatusCode.Forbidden,
                    status = "failed",
                    message = $"{ex.Message}"
                };

                var jsonRespose = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonRespose);

                return;
            }
            catch (ArgumentException ex)
            {
                context.Response.ContentType = httpContentType;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var errorResponse = new 
                {
                    code = (int)HttpStatusCode.BadRequest,
                    status = "failed",
                    message = $"{ex.Message}"
                };

                var jsonRespose = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonRespose);
                
                return;
            }
        }
    }
}