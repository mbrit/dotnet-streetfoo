﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mbrit.StreetFoo.Web.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mbrit.StreetFoo.Tests
{
    [TestClass]
    public class HelloWorldTests : TestBase
    {
        [TestMethod]
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
            string result = output.GetValueSafe<string>("result");
            Console.WriteLine(result);
            Assert.AreEqual("Hello, Martha.", result);
        }
    }
}
