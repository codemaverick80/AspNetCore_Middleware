using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Middleware.CustomMiddleware
{
    public class ExceptionHandlingMiddleware
    {
        private RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception exception)
            {
                await HandleException(httpContext.Response, exception);
            }
        }

        private static async Task HandleException(HttpResponse httpResponse, Exception exception)
        {
            httpResponse.Headers.Add("Exception-Type", exception.GetType().Name);
            httpResponse.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Huston we have a problem";
            httpResponse.StatusCode = (int)HttpStatusCode.BadRequest;
            await httpResponse.WriteAsync(exception.Message).ConfigureAwait(false);
        }
    }


    public static class AppBuilderExtension
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }

}
