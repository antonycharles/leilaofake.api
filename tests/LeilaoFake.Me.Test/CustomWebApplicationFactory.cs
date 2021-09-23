using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using LeilaoFake.Me.Service.Services;
using LeilaoFake.Me.Infra.Data.Repositories;
using LeilaoFake.Me.Infra.Datas.Migrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Microsoft.AspNetCore.Mvc.Testing;
using Dapper;

namespace LeilaoFake.Me.Test
{
    public class CustomWebApplicationFactory<TStartup>: WebApplicationFactory<TStartup> where TStartup: class
    {
        string _dataBase = "db_leilaofake_test";
        string _connectionStringAdmin = $"Host=localhost;Port=5432;Pooling=true;User Id=postgres;Password=mysecretpassword;"; 
        string _connection => $"Host=localhost;Port=5432;Pooling=true;Database={_dataBase};User Id=postgres;Password=mysecretpassword;";
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Console.WriteLine("Tenho que melhorar está classe. Chamar ela apenas uma vez por execução.");
            Console.WriteLine("Tenho que melhorar está classe. Chamar ela apenas uma vez por execução.");
            Console.WriteLine("Tenho que melhorar está classe. Chamar ela apenas uma vez por execução.");
            Console.WriteLine("Tenho que melhorar está classe. Chamar ela apenas uma vez por execução.");
            Console.WriteLine("Tenho que melhorar está classe. Chamar ela apenas uma vez por execução.");
            Console.WriteLine("Tenho que melhorar está classe. Chamar ela apenas uma vez por execução.");
            Console.WriteLine("Tenho que melhorar está classe. Chamar ela apenas uma vez por execução.");
            Console.WriteLine("Tenho que melhorar está classe. Chamar ela apenas uma vez por execução.");
            Console.WriteLine("Tenho que melhorar está classe. Chamar ela apenas uma vez por execução.");
            Console.WriteLine("Tenho que melhorar está classe. Chamar ela apenas uma vez por execução.");
            Console.WriteLine("Tenho que melhorar está classe. Chamar ela apenas uma vez por execução.");
            Console.WriteLine("Tenho que melhorar está classe. Chamar ela apenas uma vez por execução.");

            builder.ConfigureServices(services =>
            {
                //CreateDataBaseTest();

                var serviceProvider = CreateServices();

                using (var scope = serviceProvider.CreateScope())
                {
                    UpdateDatabase(scope.ServiceProvider);
                }
            });
        }

        private  IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(_connection)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(CriacaoTabelasMigration_202109172115).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        private  void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }

        public void CreateDataBaseTest()
        {
            using(var ConnectionAdmin = new NpgsqlConnection(_connectionStringAdmin))
            {
                bool dataBaseExiste = ConnectionAdmin.Query<bool>($"select exists( SELECT datname FROM pg_catalog.pg_database WHERE lower(datname) = lower('{_dataBase}'))").FirstOrDefault();

                if(dataBaseExiste)
                {
                    ConnectionAdmin.Execute($"REVOKE CONNECT ON DATABASE {_dataBase} FROM public");
                    ConnectionAdmin.Execute($"SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '{_dataBase}';");
                    ConnectionAdmin.Execute($"DROP DATABASE IF EXISTS {_dataBase}");
                }


                ConnectionAdmin.Execute($"CREATE DATABASE {_dataBase}");
            }
        }

        public IDbConnection GetConnection()
        {
            return new NpgsqlConnection(_connection); 
        }

    }
}