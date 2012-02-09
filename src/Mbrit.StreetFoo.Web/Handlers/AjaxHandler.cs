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
            }
            catch(Exception ex)
            {
                output["isOk"] = false;
                output["error"] = "General failure.";
                output["generalFailure"] = ex.ToString();
            }

            // send...
            context.Response.ContentType = "text/json";
            context.Response.Write(output.ToString());
        }

        public void DoRequest(JsonData input, JsonData output)
        {
            // check the api...
            string apiKey = input.GetValueAsString("apiKey");
            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("The 'apiKey' value was not specified in the request.");
            ApiUser api = ApiUser.GetOrCreateApiUser(new Guid(apiKey));
            if (api == null)
                throw new InvalidOperationException("'api' is null.");

            // go...
            this.DoRequest(api, input, output);
        }

        protected abstract void DoRequest(ApiUser api, JsonData input, JsonData output);
    }
}