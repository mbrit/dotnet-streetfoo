using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mbrit.StreetFoo.Entities;

namespace Mbrit.StreetFoo.Web.Handlers
{
    /// <summary>
    /// Summary description for HandleGetReports
    /// </summary>
    public class HandleGetReportsByUser : AjaxHandler
    {
        protected override void DoRequest(AjaxContext context, JsonData input, JsonData output)
        {
            if (context.User == null)
                throw new InvalidOperationException("'context.User' is null.");

            // run...
            IEnumerable<Report> reports = Report.GetByUser(context, context.User);
            output["reports"] = Entity.ToJson<Report>(reports);
        }
    }
}