using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace eCommerce.API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Logger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, Logger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                 await _next(httpContext);
                
            }
            catch(Exception ex)
            {
               _logger.LogError($"External Exception -{ex.GetType().ToString()}: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"Internal Exception - {ex.InnerException.GetType().ToString()}: {ex.InnerException.Message}");
                }

            }
            
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}



////////////////////Notes//////////////////////////////
//Constructor:

//public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
//{
//    _next = next;
//    _logger = logger;
//}
//RequestDelegate next:

//Represents the next delegate (middleware) in the pipeline.

//ILogger<ExceptionHandlingMiddleware> logger:

//Provides logging capabilities specific to this middleware.



//Invoke Method:

//public async Task Invoke(HttpContext httpContext)
//{
//    try
//    {
//        await _next(httpContext);
//    }
//    catch (Exception ex)
//    {
//        //Log the exception type and message
//        _logger.LogError($"{ex.GetType().ToString()}: {ex.Message}");

//        if (ex.InnerException is not null)
//        {
//            //Log the inner exception type and message
//            _logger.LogError($"{ex.InnerException.GetType().ToString()}: {ex.InnerException.Message}");
//        }

//        httpContext.Response.StatusCode = 500; //Internal Server Error
//        await httpContext.Response.WriteAsJsonAsync(new { Message = ex.Message, Type = ex.GetType().ToString() });
//    }
//}
//Try - Catch Block:

//await _next(httpContext);:

//Calls the next middleware in the pipeline.

//Catch Block:

//Catches any exceptions thrown by the subsequent middlewares or request handlers.

//Logging:

//Logs the type and message of the exception and any inner exceptions.

//Response:

//Sets the HTTP response status code to 500 (Internal Server Error).

//Writes a JSON response containing the exception message and type.





//ExceptionHandlingMiddlewareExtensions Class
//public static class ExceptionHandlingMiddlewareExtensions
//{
//    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
//    {
//        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
//    }
//}
//Extension Method:

//Provides a convenient way to add the ExceptionHandlingMiddleware to the HTTP request pipeline.

//UseExceptionHandlingMiddleware Method:

//this IApplicationBuilder builder:

//Allows extension methods to be called on IApplicationBuilder.

//builder.UseMiddleware<ExceptionHandlingMiddleware>():

//Adds the ExceptionHandlingMiddleware to the pipeline.





//Summary of Key Concepts of ExceptionHandlingMiddleware
//Middleware:

//Handles HTTP requests and responses, and can be used for cross-cutting concerns like exception handling.

//Exception Handling:

//Catches and manages exceptions to prevent application crashes and provide meaningful error responses.

//Logging:

//Captures details of exceptions for debugging and monitoring.

//Extension Methods:

//Provide a convenient way to configure and add middleware to the request pipeline.

//The ExceptionHandlingMiddleware ensures that exceptions are logged and handled gracefully, improving the reliability and maintainability of the application.


