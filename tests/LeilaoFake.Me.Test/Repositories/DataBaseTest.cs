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

        
        private string _dataBase;
        private string _connectionStringAdmin = $"Host=localhost;Port=5432;Pooling=true;User Id=postgres;Password=mysecretpassword;";
        private string _connection => $"Host=localhost;Port=5432;Pooling=true;Database={_dataBase};User Id=postgres;Password=mysecretpassword;";

        public DataBaseTest(string dataBase)
        {
            _dataBase = dataBase;

            this.CreateDataBaseTest();

            var serviceProvider = this.CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                this.UpdateDatabase(scope.ServiceProvider);
            }
        }

        private IServiceProvider CreateServices()
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
                //.AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        private void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }

        private void CreateDataBaseTest()
        {
            using (var ConnectionAdmin = new NpgsqlConnection(_connectionStringAdmin))
            {
                //System.Console.WriteLine($"INICIO CreateDataBaseTest ========================================================");
                bool dataBaseExiste = ConnectionAdmin.Query<bool>($"select exists( SELECT datname FROM pg_catalog.pg_database WHERE lower(datname) = lower('{_dataBase}'))").FirstOrDefault();

                //System.Console.WriteLine($"Data Base exist 1 = " + dataBaseExiste);

                if (dataBaseExiste)
                {
                    if (dataBaseExiste)
                    {
                        ConnectionAdmin.Execute($"REVOKE CONNECT ON DATABASE {_dataBase} FROM public");
                        ConnectionAdmin.Execute($"SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '{_dataBase}';");
                        ConnectionAdmin.Execute($"DROP DATABASE IF EXISTS {_dataBase}");
                    }

                    //System.Console.WriteLine($"Data Base Removido");
                }


                ConnectionAdmin.Execute($"CREATE DATABASE {_dataBase}");
                //System.Console.WriteLine($"FIM CreateDataBaseTest ========================================================");
            }
        }

        public void DropDataBase()
        {
            using (var ConnectionAdmin = new NpgsqlConnection(_connectionStringAdmin))
            {
                bool dataBaseExiste = ConnectionAdmin.Query<bool>($"select exists( SELECT datname FROM pg_catalog.pg_database WHERE lower(datname) = lower('{_dataBase}'))").FirstOrDefault();

                if (dataBaseExiste)
                {
                    ConnectionAdmin.Execute($"REVOKE CONNECT ON DATABASE {_dataBase} FROM public");
                    ConnectionAdmin.Execute($"SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '{_dataBase}';");
                    ConnectionAdmin.Execute($"DROP DATABASE IF EXISTS {_dataBase}");
                }
            }
        }

        public IDbConnection GetConnection()
        {
            return new NpgsqlConnection(_connection);
        }
    }
}