using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicRouting.routing;
using DynamicRouting.security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace DynamicRouting
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            // dynamic routing
            services.Configure<RoutingConfiguration>(Configuration.GetSection(RestConst.CONFIG_ROUTING));
            services.AddSingleton<RouteTransformer>();

            // APIKey validation
            //services.AddAuthentication(RestConst.SCHEMA_API_KEY_VALIDATION)
            //        .AddScheme<APIKeyAuthOption, APIKeyAuthHandler>(RestConst.SCHEMA_API_KEY_VALIDATION, null);

            //services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DynamicRouting", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DynamicRouting v1"));
            }

            app.UseHttpsRedirection();

            // handle routing first because key validation could be set up based on each route
            app.UseRouting();
            app.UseMiddleware<APIKeyValidator>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDynamicControllerRoute<RouteTransformer>("{path}");
            });
        }
    }
}

