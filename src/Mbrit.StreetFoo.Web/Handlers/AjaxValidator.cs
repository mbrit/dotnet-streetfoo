using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Mbrit.StreetFoo.Web
{
    internal class AjaxValidator
    {
        private List<string> Errors { get; set; }

        internal AjaxValidator()
        {
            this.Errors = new List<string>();
        }

        internal bool HasErrors
        {
            get
            {
                return this.Errors.Count != 0;
            }
        }

        internal void AddError(string error)
        {
            this.Errors.Add(error);
        }

        internal void Apply(JsonData output)
        {
            if (this.HasErrors)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string error in this.Errors)
                {
                    if (builder.Length > 0)
                        builder.Append("\n");
                    builder.Append(error);
                }

                output["error"] = builder.ToString();
                output["isOk"] = false;
            }
            else
                output["isOk"] = true;
        }
    }
}