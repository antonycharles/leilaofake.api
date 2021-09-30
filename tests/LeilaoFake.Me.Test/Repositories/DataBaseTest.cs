using System;
using System.Data;
using System.Linq;
using Dapper;
using FluentMigrator.Runner;
using LeilaoFake.Me.Infra.Datas.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace LeilaoFake.Me.Test.Repositories
{
    public  class DataBaseTest
    {

        public  string _dataBase;
        public  string _connectionStringAdmin = $"Host=localhost;Port=5432;Pooling=true;User Id=postgres;Password=mysecretpassword;"; 
        public  string _connection => $"Host=localhost;Port=5432;Pooling=true;Database={_dataBase};User Id=postgres;Password=mysecretpassword;";

        public DataBaseTest(string dataBase)
        {
            _dataBase = dataBase;
        }

        public IServiceProvider CreateServices()
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

        public void UpdateDatabase(IServiceProvider serviceProvider)
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