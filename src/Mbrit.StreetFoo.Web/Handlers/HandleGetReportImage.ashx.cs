using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Mbrit.StreetFoo.Web.Handlers
{
    public class HandleGetReportImage : AjaxHandler
    {
        protected override void DoRequest(AjaxContext context, JsonData input, JsonData output)
        {
            AjaxValidator validator = new AjaxValidator();
            var id = validator.GetRequiredString(input, "nativeId");

            if (validator.IsOk)
            {
                // what is it...
                string asString = id.Substring(id.Length - 2);
                int asInt = Convert.ToInt32(asString, 16);

                // which one...
                string resourceName = string.Format("Mbrit.StreetFoo.Resources.Graffiti{0:d2}.jpg", asInt % 5);
                var inStream = typeof(FooRuntime).Assembly.GetManifestResourceStream(resourceName);
                if (inStream == null)
                    throw new InvalidOperationException("'inStream' is null.");
                using (inStream)
                {
                    using (var outStream = new MemoryStream())
                    {
                        var bs = new byte[32768];
                        while (true)
                        {
                            int read = inStream.Read(bs, 0, bs.Length);
                            if (read == 0)
                                break;

                            outStream.Write(bs, 0, read);
                        }

                        // set...
                        output["image"] = Convert.ToBase64String(outStream.ToArray());
                    }
                }
            }

            // ok...
            validator.Apply(output);
        }
    }
}