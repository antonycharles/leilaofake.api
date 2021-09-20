using System;
using System.Data;
using System.Linq;
using Dapper;
using FluentMigrator.Runner;
using LeilaoFake.Me.Infra.Datas.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace LeilaoFake.Me.Test
{
    public static class DatabaseTest
    {
        public static IDbConnection Start()
        {
            if(_bancoExiste)
                return Connection;

            EnsureDatabase();
            var serviceProvider = CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }

            _bancoExiste = true;
            return Connection;
        }

        static string  _database => "db_teste_leilao_fake";
        static string _connectionStringAdmin => $"Host=localhost;Port=5432;Pooling=true;User Id=postgres;Password=mysecretpassword;"; 
        static string _connectionStringDataBase => $"Host=localhost;Port=5432;Pooling=true;Database={_database};User Id=postgres;Password=mysecretpassword;";
        public static IDbConnection Connection => new NpgsqlConnection(_connectionStringDataBase);

        static bool _bancoExiste = false;

        
        public static void EnsureDatabase()
        {
            using var connection = new NpgsqlConnection(_connectionStringAdmin);
            var result = connection.Query<bool>($"select exists( SELECT datname FROM pg_catalog.pg_database WHERE lower(datname) = lower('{_database}'))").FirstOrDefault();
            
            if(result){
                connection.Execute($"REVOKE CONNECT ON DATABASE {_database} FROM public");
                connection.Execute($"SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '{_database}';");
                connection.Execute($"DROP DATABASE IF EXISTS {_database}");
            }

            connection.Execute($"CREATE DATABASE {_database}");
        }

        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(_connectionStringDataBase)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(CriacaoTabelasMigration_202109172115).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }
}