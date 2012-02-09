using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mbrit.StreetFoo.Web.Handlers;

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
    }
}
