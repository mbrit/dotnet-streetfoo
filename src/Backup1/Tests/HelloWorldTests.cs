using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mbrit.StreetFoo.Web.Handlers;
using NUnit.Framework;

namespace Mbrit.StreetFoo.Tests
{
    [TestFixture()]
    public class HelloWorldTests : TestBase
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
            string result = output.GetValueSafe<string>("result");
            Console.WriteLine(result);
            Assert.AreEqual("Hello, Martha.", result);
        }
    }
}
