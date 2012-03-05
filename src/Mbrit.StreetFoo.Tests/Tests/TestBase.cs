using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mbrit.StreetFoo.Entities;
using NUnit.Framework;

namespace Mbrit.StreetFoo.Tests
{
    public abstract class TestBase : IApiUserSource
    {
        public static Random Random { get; set; }
        public ApiUser ApiUser { get; private set; }
        private static ApiUser MockApiUser { get; set; }
        internal Creator Creator { get; private set; }

        protected TestBase()
        {
            this.Creator = new Creator(this);
            this.ApiUser = MockApiUser;
        }

        static TestBase()
        {
            Random = new Random();

            // fake...
            MockApiUser = ApiUser.CreateMockApiUser();
        }

        [SetUp()]
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

        protected string ApiKey
        {
            get
            {
                return ApiUser.ApiKey;
            }
        }

        protected JsonData CreateJsonData(bool logonUser = true)
        {
            if (logonUser)
            {
                User user = this.Creator.CreateUser();
                return this.CreateJsonData(user);
            }
            else
                return this.CreateJsonData(null);
        }

        protected JsonData CreateJsonData(User user)
        {
            JsonData input = new JsonData();
            this.ApplyApiKey(input);
            if (user != null)
                this.ApplyLogonToken(input, user);

            return input;
        }

        protected void ApplyApiKey(JsonData input)
        {
            input["apiKey"] = ApiKey;
        }

        protected void ApplyLogonToken(JsonData input)
        {
            User user = this.Creator.CreateUser();
            this.ApplyLogonToken(input, user);
        }

        protected void ApplyLogonToken(JsonData input, User user)
        {
            Token token = Token.CreateToken(this, user);
            if (token == null)
                throw new InvalidOperationException("'token' is null.");

            input["logonToken"] = token.TheToken;
        }

        protected void ResetReports()
        {
            IEnumerable<Report> reports = Report.GetAll(this);
            foreach (Report report in reports)
                report.DeleteReport(this);
        }
    }
}
