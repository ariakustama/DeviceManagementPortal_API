using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System.IO;
using Microsoft.OpenApi.Models;
using DeviceManagementPortal.Helpers;
using DeviceManagementPortal.Models;
using Microsoft.EntityFrameworkCore;
using DeviceManagementPortal.Facade;

namespace DeviceManagementPortal
{
    public class Startup
    {
        private static readonly string LOGGER_OUTPUT_TEMPLATE = "[{Timestamp:o}] [{Level:u3}] ({Application}/{MachineName}/{ThreadId}) {Message}{NewLine}{Exception}";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            string loggerFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Log", $"Api_DeviceManagementPortal.log");
            Log.Logger = CreateDefaultLogger(configuration, loggerFilePath);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc();

            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(options => options.UseSqlite(connection));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Device Management Api", Version = "v1" });
            });

            services.Configure<ApiBehaviorOptions>(a =>
            {
                a.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new CustomBadRequest(context);
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application / json" }
                    };
                };
            });

            services.AddTransient<GlobalFacade>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Device Management Api V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });
        }

        private static Logger CreateDefaultLogger(IConfiguration config, string loggerFilePath) =>
            new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Application", "Api_DeviceManagementPortal")
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .WriteTo.Console(outputTemplate: LOGGER_OUTPUT_TEMPLATE)
                //.WriteTo.File(loggerFilePath, rollingInterval: RollingInterval.Day, outputTemplate: LOGGER_OUTPUT_TEMPLATE, fileSizeLimitBytes: 512000000, rollOnFileSizeLimit: true)
                .ReadFrom.Configuration(config)
                .CreateLogger();
    }
}
