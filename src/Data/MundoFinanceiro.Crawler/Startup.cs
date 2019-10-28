using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MundoFinanceiro.Database.Contracts.Persistence;
using MundoFinanceiro.Database.Persistence;
using MundoFinanceiro.Shared.Attributes;
using MundoFinanceiro.Shared.Constants;

namespace MundoFinanceiro.Crawler
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
            services.AddCors();
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.WriteIndented = false;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });
            
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSwaggerGen(conf =>
            {
                conf.SwaggerDoc(VersionConstants.V1, new OpenApiInfo
                {
                    Title = $"MundoFinanceiro.Crawler ({VersionConstants.V1})",
                    Version = VersionConstants.V1,
                    Description = "API utilizada para realizar a requisição da mineração dos fundamentos..",
                    Contact = new OpenApiContact
                    {
                        Name = "Daniel Cunha",
                        Url = new Uri("https://www.linkedin.com/in/daniel-cunha-53053816b/"),
                        Email = "danielcunha54@gmail.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://github.com/danielccunha/mundo-financeiro/blob/master/LICENSE")
                    }
                });
            });
            
            ConfigureContainer(services);
        }

        private void ConfigureContainer(IServiceCollection services)
        {
            // Database
            var connectionString = Configuration.GetConnectionString(ConfigurationConstants.ConnectionString);
            if (string.IsNullOrWhiteSpace(connectionString)) 
                throw new ArgumentNullException(nameof(connectionString), @"A connection string não pode ser nula.");
            
            services.AddScoped<IUnitOfWork>(provider => new UnitOfWork(connectionString));
            
            // Mapped Services
            ConfigureMappedServices(services);
        }
        
        /// <summary>
        /// Configura os serviços que possuem o atributo MappedService. 
        /// </summary>
        /// <param name="services">Collection de serviços da aplicação</param>
        private static void ConfigureMappedServices(IServiceCollection services)
        {
            var attributeType = typeof(MappedServiceAttribute);
            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetCustomAttributes(attributeType, true).Length > 0);

            foreach (var type in types)
            {
                var inheritedType = type
                    .GetInterfaces()
                    .Single(x => x.Name.Contains(type.Name));

                services.AddScoped(inheritedType, type);
            }
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{VersionConstants.V1}/swagger.json", $"MundoFinanceiro.Crawler ({VersionConstants.V1})"));
        }
    }
}