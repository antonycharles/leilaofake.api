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
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using LeilaoFake.Me.Api.Token;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IO;

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


            var tokenConfigurations = new TokenConfigurations();

            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
                    
            services.AddSingleton(tokenConfigurations);


            var key = Encoding.ASCII.GetBytes(tokenConfigurations.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = tokenConfigurations.Issuer,
                    ValidAudience = tokenConfigurations.Audience
                };
            });

            services.AddTransient<ITokenService,TokenService>();

            services.AddFluentMigratorCore()
                .ConfigureRunner(cfg => cfg
                    .AddPostgres()
                    .WithGlobalConnectionString(dbConnectionString)
                    .ScanIn(typeof(CriacaoTabelasMigration_202109172115).Assembly).For.Migrations()
                )
                .AddLogging(cfg => cfg.AddFluentMigratorConsole());

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.AddTransient<ILeilaoRepository,LeilaoRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<ILanceRepository, LanceRepository>();

            services.AddTransient<IUsuarioService,UsuarioService>();
            services.AddTransient<ILeilaoService, LeilaoService>();
            services.AddTransient<ILanceService, LanceService>();


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers(options => {
                options.Filters.Add(typeof(ErrorResponseFilter));
            });

            services.AddCors(options => options.AddDefaultPolicy(
                builder => builder.AllowAnyOrigin()
            ));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>{
                c.SwaggerDoc("v1", new OpenApiInfo 
                {
                    Version = "v1",
                    Title = "Api Leilão Fake",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce leo elit, interdum id urna non, facilisis cursus neque. Mauris sed libero eros. Phasellus facilisis nulla quis justo feugiat mollis. Maecenas euismod semper lacinia. Curabitur sed nunc ac purus hendrerit fringilla a in neque. Curabitur lacinia nunc orci, nec venenatis libero malesuada sed. Nulla eu viverra libero, cursus gravida felis. Morbi ut odio dapibus, consequat dolor non, sollicitudin ante. Morbi nisl metus, luctus quis viverra sit amet, pulvinar placerat quam.",
                    Contact = new OpenApiContact
                    {
                        Name = "Antony Reis",
                        Email = "antony.reis.dev@gmail.com",
                        Url = new Uri("https://antonycharles.com.br/"),
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            migrationRunner.MigrateUp();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            /*app.Use((context, next) =>
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin","*");
                context.Response.Headers.Add("Access-Control-Allow-Credentials","true");
                return next();
            });*/

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}