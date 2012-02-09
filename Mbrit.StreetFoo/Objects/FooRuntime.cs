using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mbrit.StreetFoo
{
    public static class FooRuntime
    {
        private static string ConnectionUrl { get; set; }
        private static string DatabaseName { get; set; }

        public static void Start(string url)
        {
            ConnectionUrl = url;

            // get...
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
