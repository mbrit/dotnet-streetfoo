using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mbrit.StreetFoo.Entities;

namespace Mbrit.StreetFoo.Web.Handlers
{
    /// <summary>
    /// Summary description for HandleCreateReport
    /// </summary>
    public class HandleCreateReport : AjaxHandler
    {
        protected override void DoRequest(AjaxContext context, JsonData input, JsonData output)
        {
            // get...
            AjaxValidator validator = new AjaxValidator();
            string title = validator.GetRequiredString(input, "title");
            string description = validator.GetRequiredString(input, "description");

            // ok?
            if (validator.IsOk)
            {
                // lat/long - tbd...
                Report report = Report.CreateReport(context, context.User, title, description, 0, 0);
                if (report == null)
                    throw new InvalidOperationException("'report' is null.");

                // set...
                output["reportId"] = report._id.ToString();
            }

            // set...
            validator.Apply(output);
        }
    }
}