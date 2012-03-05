using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mbrit.StreetFoo.Entities;

namespace Mbrit.StreetFoo.Web
{
    public abstract class AjaxHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string json = null;
            using (Stream stream = context.Request.InputStream)
            {
                StreamReader reader = new StreamReader(stream);
                json = reader.ReadToEnd();
            }

            // load...
            JsonData input = new JsonData(json);
            JsonData output = new JsonData();
            try
            {
                DoRequest(input, output);

                // did we get output?
                if (!(output.ContainsKey("isOk")))
                    output["isOk"] = true;
            }
            catch(Exception ex)
            {
                output["isOk"] = false;
                output["error"] = "General failure.";
                output["generalFailure"] = ex.ToString();
            }

            // jqm access control bits...
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            context.Response.AddHeader("Access-Control-Allow-Methods", "POST, GET");

            // send...
            context.Response.ContentType = "text/json";
            context.Response.Write(output.ToString());
        }

        public void DoRequest(JsonData input, JsonData output)
        {
            AjaxContext context = new AjaxContext(input);
            this.DoRequest(context, input, output);
        }
  
        protected abstract void DoRequest(AjaxContext context, JsonData input, JsonData output);
    }
}