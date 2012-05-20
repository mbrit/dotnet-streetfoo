using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mbrit.StreetFoo.Web.Handlers
{
    public class HelloWorldHandler : AjaxHandler
    {
        protected override void DoRequest(AjaxContext context, JsonData input, JsonData output)
        {
            // get...
            string name = input.GetValueSafe<string>("name");

            // return...
            output["result"] = string.Format("Hello, {0}.", name);
        }
    }
}