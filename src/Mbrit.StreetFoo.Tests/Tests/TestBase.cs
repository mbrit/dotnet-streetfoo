using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mbrit.StreetFoo.Entities;

namespace Mbrit.StreetFoo.Tests
{
    public abstract class TestBase
    {
        public static Random Random { get; set; }
        private static ApiUser MockApiUser { get; set; }
        internal Creator Creator { get; private set; }

        protected TestBase()
        {
            this.Creator = new Creator(this);
        }

        static TestBase()
        {
            Random = new Random();

            // fake...
            MockApiUser = ApiUser.CreateMockApiUser();
        }

        [TestInitialize()]
        public void Initialize()
        {
            // if...
            if (!(FooRuntime.IsStarted))
                FooRuntime.Start("mongodb://StreetFooTestUser:8o9sK3X52F76jo68bPlR3nlD3E1224@staff.mongohq.com:10059/StreetFooTest");
        }

        internal string GetRandomId(string name)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(name);
            for (int index = 0; index < 4; index++)
                builder.Append((char)Random.Next(48, 57));
            for (int index = 0; index < 4; index++)
                builder.Append((char)Random.Next(65, 91));

            return builder.ToString();
        }

        protected ApiUser ApiUser
        {
            get
            {
                return MockApiUser;
            }
        }

        protected string ApiKey
        {
            get
            {
                return ApiUser.ApiKey;
            }
        }

        protected void ApplyApiKey(JsonData input)
        {
            input["apiKey"] = ApiKey;
        }
    }
}
