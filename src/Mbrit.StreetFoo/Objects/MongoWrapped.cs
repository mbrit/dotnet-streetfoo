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
        private string ConnectionString { get; set; }
        private string DatabaseName { get; set; }
        private MongoServer Server { get; set; }
        internal MongoDatabase Database { get; set; }

        internal MongoWrapped(string connString, string databaseName)
        {
            try
            {
                this.Server = MongoServer.Create(connString);
                this.Server.Connect();

                // get...
                this.Database = this.Server.GetDatabase(databaseName);
                if (this.Database == null)
                    throw new InvalidOperationException(string.Format("A database with name '{0}' was not found.", databaseName));
            }
            catch (Exception ex)
            {
                throw WrapError(ex);
            }
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

        internal Exception WrapError(Exception ex)
        {
            return new InvalidOperationException(string.Format("Failed to issue Mongo instruction.\r\nURL: {0}\r\nDatabase: {1}",
                this.ConnectionString, this.DatabaseName), ex);
        }
    }
}
