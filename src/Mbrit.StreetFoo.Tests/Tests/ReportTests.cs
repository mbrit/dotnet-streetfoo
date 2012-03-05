using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Mbrit.StreetFoo.Web.Handlers;
using Mbrit.StreetFoo.Entities;
using NUnit.Framework;

namespace Mbrit.StreetFoo.Tests
{
    [TestFixture()]
    public class ReportTests : TestBase
    {
        [Test()]
        public void TestCreateReport()
        {
            JsonData input = CreateJsonData();
            input["title"] = GetRandomId("title");
            input["description"] = GetRandomId("description");

            // go...
            HandleCreateReport handler = new HandleCreateReport();
            JsonData output = new JsonData();
            handler.DoRequest(input, output);

            // check...
            Assert.AreEqual(true, output["isOk"]);
            Assert.IsNull(output["error"]);
            Assert.IsNotNull(output["reportId"]);
        }

        [Test()]
        public void TestGetReportsByUser()
        {
            ResetReports();

            // create some reports..
            User user = this.Creator.CreateUser();
            Report report1 = this.Creator.CreateReport(user);
            Report report2 = this.Creator.CreateReport(user);
            Report report3 = this.Creator.CreateReport(user);
            Report report4 = this.Creator.CreateReport(user);
            Report report5 = this.Creator.CreateReport(user);

            // get...
            HandleGetReportsByUser handler = new HandleGetReportsByUser();
            JsonData output = new JsonData();
            handler.DoRequest(this.CreateJsonData(user), output);

            // check...
            string asString = output.GetValueAsString("reports");
            IList reports = (IList)new JavaScriptSerializer().DeserializeObject(asString);
            Assert.AreEqual(5, reports.Count);

            // check...
            Assert.AreEqual(this.ApiKey, ((IDictionary)reports[0])["apiKey"]);
            Assert.AreEqual(user.IdAsString, ((IDictionary)reports[0])["ownerUserId"]);
            Assert.AreEqual(this.ApiKey, ((IDictionary)reports[1])["apiKey"]);
            Assert.AreEqual(user.IdAsString, ((IDictionary)reports[1])["ownerUserId"]);
            Assert.AreEqual(this.ApiKey, ((IDictionary)reports[2])["apiKey"]);
            Assert.AreEqual(user.IdAsString, ((IDictionary)reports[2])["ownerUserId"]);
            Assert.AreEqual(this.ApiKey, ((IDictionary)reports[3])["apiKey"]);
            Assert.AreEqual(user.IdAsString, ((IDictionary)reports[3])["ownerUserId"]);
            Assert.AreEqual(this.ApiKey, ((IDictionary)reports[4])["apiKey"]);
            Assert.AreEqual(user.IdAsString, ((IDictionary)reports[4])["ownerUserId"]);
        }

        [Test()]
        public void TestEnsureTestReports()
        {
            ResetReports();

            // create some reports..
            User user = this.Creator.CreateUser();

            // check...
            Assert.AreEqual(0, user.GetReports(this).Count<Report>());

            // get...
            HandleEnsureTestReports handler = new HandleEnsureTestReports();
            JsonData output = new JsonData();
            handler.DoRequest(this.CreateJsonData(user), output);

            // check...
            string asString = output.GetValueAsString("reports");
            IList reports = (IList)new JavaScriptSerializer().DeserializeObject(asString);
            Assert.AreEqual(50, reports.Count);
        }
    }
}
