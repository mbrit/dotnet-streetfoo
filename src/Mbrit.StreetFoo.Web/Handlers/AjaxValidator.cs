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
        private List<string> Missings { get; set; }

        internal AjaxValidator()
        {
            this.Errors = new List<string>();
            this.Missings = new List<string>();
        }

        internal bool IsOk
        {
            get
            {
                return this.Errors.Count == 0 && this.Missings.Count == 0;
            }
        }

        internal void AddError(string error)
        {
            this.Errors.Add(error);
        }

        internal void Apply(JsonData output)
        {
            FlushMissings();

            if(this.Errors.Count > 0)
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

        private void FlushMissings()
        {
            if (this.Missings.Count == 0)
                return;

            StringBuilder builder = new StringBuilder();
            foreach (string missing in Missings)
            {
                if (builder.Length > 0)
                    builder.Append(", ");
                builder.Append(missing);
            }

            if (this.Missings.Count == 1)
                builder.Append(" is required.");
            else
                builder.Append(" are required.");

            this.Errors.Add(builder.ToString());
            this.Missings.Clear();
        }  

        internal string GetRequiredString(JsonData input, string name)
        {
            object value = GetRequiredValue(input, name);
            return Convert.ToString(value);
        }

        private object GetRequiredValue(JsonData input, string name)
        {
            object value = input.GetValueSafe(name);
            if(value == null || (value is string && ((string)value).Length == 0))
                this.Missings.Add(name);

            return value;
        }
    }
}