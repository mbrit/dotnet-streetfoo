using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using Mbrit.StreetFoo.Entities;

namespace Mbrit.StreetFoo
{
    internal class MongoWrapped : IDisposable
    {
        private MongoServer Server { get; set; }
        internal MongoDatabase Database { get; set; }

        internal MongoWrapped(string connString, string databaseName)
        {
            this.Server = MongoServer.Create(connString);
            this.Server.Connect();

            // get...
            this.Database = this.Server.GetDatabase(databaseName);
            if (this.Database == null)
                throw new InvalidOperationException(string.Format("A database with name '{0}' was not found.", databaseName));
        }

        ~MongoWrapped()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (this.Server != null)
            {
                try
                {
                    this.Server.Disconnect();
                }
                finally
                {
                    this.Server = null;
                }
            }

            // sup...
            GC.SuppressFinalize(this);
        }

        internal MongoCollection<T> GetCollection<T>()
            where T : Entity
        {
            return this.Database.GetCollection<T>(GetCollectionName<T>());
        }

        private string GetCollectionName<T>()
            where T : Entity
        {
            return typeof(T).Name;
        }
    }
}
