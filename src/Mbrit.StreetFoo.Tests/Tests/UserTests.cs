﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mbrit.StreetFoo.Web.Handlers;
using Mbrit.StreetFoo.Entities;
using NUnit.Framework;
using System.Diagnostics;

namespace Mbrit.StreetFoo.Tests
{
    [TestFixture()]
    public class UserTests : TestBase
    {
        [Test()]
        public void TestHelloWorld()
        {
            JsonData input = new JsonData();
            ApplyApiKey(input);
            input["name"] = "Martha";

            // run...
            HelloWorldHandler handler = new HelloWorldHandler();
            JsonData output = new JsonData();
            handler.DoRequest(input, output);

            // dump...
            string result = output.GetValueAsString("result");
            Console.WriteLine(result);
            Assert.AreEqual("Hello, Martha.", result);
        }

        // [test()]
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

        // [test()]
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

        // [test()]
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

        // [test()]
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

        // [test()]
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

        // [test()]
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
