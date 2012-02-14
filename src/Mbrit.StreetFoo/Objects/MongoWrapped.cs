using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using Mbrit.StreetFoo.Entities;
using System.Text.RegularExpressions;

namespace Mbrit.StreetFoo
{
    internal class MongoWrapped : IDisposable
    {
        private string ConnectionString { get; set; }
        private string DatabaseName { get; set; }
        private MongoServer Server { get; set; }
        internal MongoDatabase Database { get; set; }

        private static Regex PasswordRegex = new Regex(@"(?<before>mongodb://[^:]+:)(?<password>[^@]+)(?<after>.*)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        internal MongoWrapped(string connString, string databaseName)
        {
            this.ConnectionString = connString;
            this.DatabaseName = databaseName;

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
                StripPassword(this.ConnectionString), this.DatabaseName), ex);
        }

        private string StripPassword(string url)
        {
            return PasswordRegex.Replace(url, new MatchEvaluator(PasswordEvaluator));
        }

        private string PasswordEvaluator(Match match)
        {
            return string.Concat(match.Groups["before"].Value, "***", match.Groups["after"].Value);
        }
    }
}
