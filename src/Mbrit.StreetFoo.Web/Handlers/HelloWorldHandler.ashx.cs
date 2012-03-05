using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mbrit.StreetFoo.Web.Handlers
{
    public class HelloWorldHandler : AjaxHandler
    {
        protected override void DoRequest(Entities.ApiUser api, JsonData input, JsonData output)
        {
            // get...
            string name = input.GetValueAsString("name");

            // return...
            output["result"] = string.Format("Hello, {0}.", name);
        }
    }
}