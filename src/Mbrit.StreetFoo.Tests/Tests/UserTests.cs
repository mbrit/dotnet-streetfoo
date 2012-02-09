using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mbrit.StreetFoo.Web.Handlers;
using Mbrit.StreetFoo.Entities;

namespace Mbrit.StreetFoo.Tests
{
    [TestClass()]
    public class UserTests : TestBase
    {
        [TestMethod()]
        public void TestHandleRegister()
        {
            JsonData input = new JsonData();
            ApplyApiKey(input);
            input["username"] = GetRandomId("user");
            input["email"] = GetRandomId("email");
            input["password"] = GetRandomId("password");

            // go...
            HandleRegister handler = new HandleRegister();
            JsonData output = new JsonData();
            handler.DoRequest(input, output);

            // check...
            Assert.AreEqual(true, output["isOk"]);
            Assert.IsNull(output["error"]);
            Assert.IsNotNull(output["userId"]);
        }

        [TestMethod()]
        public void TextHandleRegisterExisingFails()
        {
            JsonData input = new JsonData();
            ApplyApiKey(input);
            input["username"] = GetRandomId("user");
            input["email"] = GetRandomId("email");
            input["password"] = GetRandomId("password");

            // go...
            HandleRegister handler = new HandleRegister();
            JsonData output = new JsonData();
            handler.DoRequest(input, output);

            // again...
            output = new JsonData();
            handler.DoRequest(input, output);

            // check...
            Assert.AreEqual(false, output["isOk"]);
            Assert.IsNotNull(output["error"]);
            Assert.IsNull(output["userId"]);
        }

        [TestMethod()]
        public void TestHandleRegisterMissingFields()
        {
            JsonData input = new JsonData();
            ApplyApiKey(input);

            // miss out the fields!

            // go...
            HandleRegister handler = new HandleRegister();
            JsonData output = new JsonData();
            handler.DoRequest(input, output);

            // check...
            Assert.AreEqual(false, output["isOk"]);
            Assert.IsNotNull(output["error"]);
            Assert.IsNull(output["userId"]);
        }

        [TestMethod()]
        public void TestLogonOk()
        {
            // create a user...
            string password = this.GetRandomId("password");
            User user = this.Creator.CreateUserWithPassword(this.ApiUser, password);

            // go...
            JsonData input = new JsonData();
            ApplyApiKey(input);
            input["username"] = user.Username;
            input["password"] = password;

            // go...
            HandleLogon handler = new HandleLogon();
            JsonData output = new JsonData();
            handler.DoRequest(input, output);

            // check...
            Assert.AreEqual(true, output["isOk"]);
            Assert.IsNull(output["error"]);
            Assert.IsNotNull(output["token"]);
        }

        [TestMethod()]
        public void TestLogonInvalidUser()
        {
            // create a user...
            string password = this.GetRandomId("password");
            User user = this.Creator.CreateUserWithPassword(this.ApiUser, password);

            // go...
            JsonData input = new JsonData();
            ApplyApiKey(input);
            input["username"] = user.Username + "xxx";  // invalid username...
            input["password"] = password;

            // go...
            HandleLogon handler = new HandleLogon();
            JsonData output = new JsonData();
            handler.DoRequest(input, output);

            // check...
            Assert.AreEqual(false, output["isOk"]);
            Assert.IsNotNull(output["error"]);
            Assert.IsNull(output["token"]);
        }

        [TestMethod()]
        public void TestLogonInvalidPassword()
        {
            // create a user...
            string password = this.GetRandomId("password");
            User user = this.Creator.CreateUserWithPassword(this.ApiUser, password);

            // go...
            JsonData input = new JsonData();
            ApplyApiKey(input);
            input["username"] = user.Username;
            input["password"] = password  + "xxx";  // invalid password...

            // go...
            HandleLogon handler = new HandleLogon();
            JsonData output = new JsonData();
            handler.DoRequest(input, output);

            // check...
            Assert.AreEqual(false, output["isOk"]);
            Assert.IsNotNull(output["error"]);
            Assert.IsNull(output["token"]);
        }
    }
}
