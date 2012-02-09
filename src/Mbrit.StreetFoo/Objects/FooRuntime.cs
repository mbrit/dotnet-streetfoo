using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Mbrit.StreetFoo
{
    public static class FooRuntime
    {
        private static string ConnectionUrl { get; set; }
        private static string DatabaseName { get; set; }

        public static void Start()
        {
            string url = ConfigurationManager.AppSettings["MongoUrl"];
            if (url == null)
                throw new InvalidOperationException("'url' is null.");
            if (url.Length == 0)
                throw new InvalidOperationException("'url' is zero-length.");

            // defer...
            Start(url);
        }

        public static void Start(string url)
        {
            if (url == null)
                throw new ArgumentNullException("url");
            if (url.Length == 0)
                throw new ArgumentException("'url' is zero-length.");

            // go...
            ConnectionUrl = url;
            DatabaseName = new Uri(url).LocalPath.Substring(1);
        }

        public static bool IsStarted
        {
            get
            {
                return !(string.IsNullOrEmpty(ConnectionUrl));
            }
        }

        internal static MongoWrapped GetDatabase()
        {
            return new MongoWrapped(ConnectionUrl, DatabaseName);
        }
    }
}
