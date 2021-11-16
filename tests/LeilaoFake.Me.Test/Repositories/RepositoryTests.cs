using System;
using System.Data;

namespace LeilaoFake.Me.Test.Repositories
{
    public abstract class RepositoryTests : IDisposable
    {
        protected RepositoryTests(string dataBase, bool deleteDataBase)
        {
            _deleteDataBase = deleteDataBase;
            Random randNum = new Random();

            _database = new DataBaseTest(dataBase + randNum.Next(1000));

            this.Context = _database.GetConnection();
        }

        public void Dispose()
        {
            if(_deleteDataBase)
                _database.DropDataBase();
        }

        private bool _deleteDataBase;
        private DataBaseTest _database;
        protected IDbConnection Context { get; set; }
    }
}