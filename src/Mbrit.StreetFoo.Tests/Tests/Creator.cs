using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mbrit.StreetFoo.Entities;

namespace Mbrit.StreetFoo.Tests
{
    internal class Creator
    {
        private TestBase TheTest { get; set; }

        internal Creator(TestBase theTest)
        {
            this.TheTest = theTest;
        }

        internal User CreateUserWithPassword(ApiUser api, string password)
        {
            return User.CreateUser(api, this.TheTest.GetRandomId("username"), this.TheTest.GetRandomId("email"), password);
        }
    }
}
