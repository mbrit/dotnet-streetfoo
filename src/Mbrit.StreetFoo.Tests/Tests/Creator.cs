using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mbrit.StreetFoo.Entities;

namespace Mbrit.StreetFoo.Tests
{
    internal class Creator : IApiUserSource
    {
        private TestBase TheTest { get; set; }

        internal Creator(TestBase theTest)
        {
            this.TheTest = theTest;
        }

        internal User CreateUser()
        {
            string password = this.TheTest.GetRandomId("password");
            return this.CreateUserWithPassword(password);
        }

        internal User CreateUserWithPassword(string password)
        {
            return User.CreateUser(this, this.TheTest.GetRandomId("username"), this.TheTest.GetRandomId("email"), password);
        }

        internal Report CreateReport(User user)
        {
            return Report.CreateReport(this, user, this.TheTest.GetRandomId("title"), this.TheTest.GetRandomId("description"), 0M, 0M);
        }

        public ApiUser ApiUser
        {
            get
            {
                return this.TheTest.ApiUser;
            }
        }
    }
}
