using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Middleware.CustomMiddleware;

namespace Middleware
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*
            * In ASP .Net core, Order of middleware matters
            * 
            *  Run() : Delegates terminates the request pipeline
            * 
            *  Use() : Multiple request delegates can be chained (by using next.Invoke())
            *  
            *  Short-Circutting : When a delegate does not call the "next" delegate
            * 
            */

            #region "Custom Middleware"

            /*
             * Pre-requisites for custom middleware class
             *  - public constructor with RequestDelegate argument
             *  - public method named Invoke or InvokeAsync
             *      - First argument must be HttpContext
             *      - Must return as Task
             *      - Additional arguments can be injected 
             * 
             * Note : Runtime creates one instance of your middleware class (one per application lifetime) - at start up
             * 
             */

            //app.UseMiddleware<ExceptionHandlingMiddleware>();

            ////OR using extension method
            app.UseCustomExceptionHandler();

            #endregion




            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            #region "Use and Run Middleware"

            //app.Use(async (context, next) =>
            //{
            //    /* Request way in => */
            //    await context.Response.WriteAsync("<h1> Incoming : Hello from middleware 1 </h1>");

            //    await next.Invoke(); // calling next middleware in the pipeline

            //    /* <= Request way out */
            //    await context.Response.WriteAsync("<h1> Outgoing : Hello from middleware 1 </h1>");

            //});

            //app.Use(async (context, next) =>
            //{
            //    /* Request way in => */
            //    await context.Response.WriteAsync("<h2> Incoming : Hello from middleware 2 </h2>");

            //    await next.Invoke(); // calling next middleware in the pipeline

            //    /* <= Request way out  */
            //    await context.Response.WriteAsync("<h2> Outgoing : Hello from middleware 2 </h2>");

            //});

            //app.Use(async (context, next) =>
            //{
            //    /* Request way in => */
            //    await context.Response.WriteAsync("<h3> Incoming : Hello from middleware 3 </h3>");

            //    await next.Invoke(); // calling next middleware in the pipeline

            //   /* <= Request way out */
            //    await context.Response.WriteAsync("<h3> Outgoing : Hello from middleware 3 </h3>");

            //});

            //app.Run(async(context) =>
            //{
            //    /* Request way in => */
            //    await context.Response.WriteAsync("<h4> Incoming : Hello from middleware 4 </h4>");


            //    await context.Response.WriteAsync("This 1st middleware using Run()");

            //    /* <= Request way out */
            //    await context.Response.WriteAsync("<h4> Incoming : Hello from middleware 4 </h4>");
            //});

            ///* This middleware will never be called */
            //app.Run(async (context) => {

            //    await context.Response.WriteAsync("<h5> Incoming : Hello from middleware 5 </h5>");
            //    await context.Response.WriteAsync("This 1st middleware using Run()");
            //    await context.Response.WriteAsync("<h5> Incoming : Hello from middleware 5 </h5>");
            //});

            #endregion

                     

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
