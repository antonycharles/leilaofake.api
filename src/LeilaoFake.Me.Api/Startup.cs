using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using LeilaoFake.Me.Api.Responses;
using LeilaoFake.Me.Service.Services;
using LeilaoFake.Me.Infra.Data.Repositories;
using LeilaoFake.Me.Infra.Datas.Migrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace LeilaoFake.Me.Api
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
            // Read the connection string from appsettings.
            string dbConnectionString = this.Configuration.GetConnectionString("PostgresConnection");

            // Inject IDbConnection, with implementation from SqlConnection class.
            services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(dbConnectionString));


            services.AddFluentMigratorCore()
                .ConfigureRunner(cfg => cfg
                    .AddPostgres()
                    .WithGlobalConnectionString(dbConnectionString)
                    .ScanIn(typeof(CriacaoTabelasMigration_202109172115).Assembly).For.Migrations()
                )
                .AddLogging(cfg => cfg.AddFluentMigratorConsole());

            services.AddTransient<ILeilaoRepository,LeilaoRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<ILanceRepository, LanceRepository>();

            services.AddTransient<IUsuarioService,UsuarioService>();

            services.AddApiVersioning();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers(options => {
                options.Filters.Add(typeof(ErrorResponseFilter));
            });

            //https://github.com/domaindrivendev/Swashbuckle.AspNetCore
            services.AddSwaggerGen(sg =>
            {
                sg.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Leilao Fake",
                    Description = "Documentacao da Api.",
                    Version = "1.0"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            migrationRunner.MigrateUp();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(sgu => sgu.SwaggerEndpoint("/swagger/v1/swagger.json", "Versao 1.0"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
//https://renatogroffe.medium.com/net-5-dapper-exemplos-de-implementa%C3%A7%C3%A3o-c3a9ce2be802