using GettingHip.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Security.Claims;

namespace GettingHip
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddSingleton<IConfiguration>(Configuration);
            services.Configure<HipConfig>(Configuration.GetSection("hipsters"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.Use((context, next) =>
            {
                context.Response.Headers.Add(new KeyValuePair<string, StringValues>("hipness", new StringValues("tremendous-amounts")));

                var claims = new Claim[] 
                {
                    new Claim(ClaimTypes.Name, "JoShmo"),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                context.User = new ClaimsPrincipal(new ClaimsIdentity(claims));

                return next();
            });

            app.UseMvc();
        }
    }
}
