using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Seventh.DGuard.Business;
using Seventh.DGuard.Database;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;

namespace Seventh.DGuard
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
            services.AddDbContext<SeventhDGuardContext>(o => o.UseSqlServer(Configuration.GetConnectionString("cs_seventh_dguard")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);

            #region Mapper

            var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfiles>();
                });
            var mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);

            #endregion

            ServicesInject.Execute(services);
            SwaggerConfig.Execute(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DefaultModelsExpandDepth(-1);
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIs para camada de negócio");
                c.DocExpansion(DocExpansion.None);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            InitializeDatabase(app);
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetRequiredService<SeventhDGuardContext>().Database.Migrate();
        }
    }
}
