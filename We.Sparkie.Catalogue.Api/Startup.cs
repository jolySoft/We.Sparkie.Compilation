using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using We.Sparkie.Catalogue.Api.Repository;

namespace We.Sparkie.Catalogue.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            var connectionString = Configuration["CATALOGUE_DB_CONNECTION_STRING"];
            services.AddDbContext<CatalogueDbContext>(cfg =>
                cfg.UseSqlServer(connectionString));

            services.AddMvc();
            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalogue Service", Version = "v1" }); });

            services.AddScoped(typeof(Repository<>));

            OnServiceCollection?.Invoke(services);
        }

        public static Action<IServiceCollection> OnServiceCollection { get; set; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital Asset API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(ep => ep.MapControllers());

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
