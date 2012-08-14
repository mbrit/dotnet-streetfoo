using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mbrit.StreetFoo.Entities;
using System.Text;

namespace Mbrit.StreetFoo.Web.Handlers
{
    /// <summary>
    /// Summary description for HandleEnsureTestData
    /// </summary>
    public class HandleEnsureTestReports : AjaxHandler
    {
        private static List<decimal> Latitudes = new List<decimal> { 51.99065M };
        private static List<decimal> Longitude = new List<decimal> { -0.75619M };

        protected override void DoRequest(AjaxContext context, JsonData input, JsonData output)
        {
            if (context.User == null)
                throw new InvalidOperationException("'context.User' is null.");

            // does the user have any reports?
            if (!(context.User.HasReports(context)))
            {
                // create...
                Random rand = new Random();
                string[] verbs = { "Fix", "Repair", "Remove", "Clean" };
                string[] adjectives = { "Broken", "Damaged", "Inappropriate" };
                string[] nouns = { "Light", "Kerb", "Wall", "Sign", "Pavement" };

                string[] lipsums = { "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec placerat purus sed orci lobortis id aliquet metus pharetra. Phasellus eget viverra magna. Cras lobortis sapien blandit erat aliquam lacinia. Vivamus nec dolor vel turpis tincidunt cursus. Nunc porta, augue nec feugiat eleifend, ante lectus porta dui, vitae rutrum urna mi laoreet nunc. Suspendisse ante justo, rutrum nec varius consequat, condimentum a mi. Donec adipiscing aliquam blandit. Phasellus eu lacus et augue vulputate porttitor nec sed justo. Fusce elit dolor, placerat quis ornare nec, dignissim vitae est.",
                    "Mauris fermentum ultrices posuere. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Ut tellus nulla, mollis et porta a, iaculis eu ipsum. Vestibulum scelerisque, sem a ullamcorper dapibus, est dolor fringilla est, vitae bibendum diam purus at eros. Proin fermentum est id risus cursus nec ullamcorper tortor dictum. Sed velit nisl, egestas vel varius nec, rutrum vel arcu. Mauris iaculis vehicula mi, hendrerit mattis purus faucibus id. Nunc suscipit urna eget libero accumsan tincidunt molestie leo volutpat. Nunc porta lacus justo. In luctus diam nec lacus vestibulum euismod.",
                    "In malesuada vulputate ipsum sed posuere. Cras pretium venenatis tellus, quis vehicula est auctor vel. In posuere adipiscing urna, ac tempor enim mollis ac. Cras vehicula pulvinar tellus quis blandit. Suspendisse potenti. Phasellus sodales imperdiet venenatis. Quisque pulvinar facilisis orci, nec venenatis arcu faucibus non. Aliquam vehicula ante id nisl facilisis cursus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Suspendisse tellus neque, imperdiet a rhoncus vitae, vestibulum ac sapien. Donec vitae sapien non mauris fermentum vulputate non ac dui. Duis hendrerit purus id justo volutpat tristique. Cras ut turpis tellus, vel dapibus diam.",
                    "Curabitur gravida elementum auctor. Vestibulum euismod, nisi in elementum dignissim, nisl quam laoreet magna, ut iaculis dolor massa ut urna. Cras velit turpis, laoreet nec rhoncus id, porttitor id velit. Ut non mi non quam interdum elementum. Morbi nec lectus neque. Donec bibendum, mi et condimentum pellentesque, felis lorem ultricies nisl, at sollicitudin magna arcu ut velit. Curabitur vitae lacus quis tellus fringilla consectetur. Cras malesuada enim eu quam elementum a lobortis libero cursus. Duis volutpat, libero ut placerat mollis, sem tortor condimentum justo, ut tincidunt risus diam ultricies dolor. Nullam mi dolor, ultricies et iaculis ut, accumsan sed tortor.",
                    "Donec in eros ac sem sollicitudin euismod eu nec magna. Quisque placerat congue arcu sit amet tincidunt. Vestibulum vitae metus metus. Nulla vitae turpis quam. Nunc eu elit quam. Sed condimentum congue elit, sed lobortis orci congue eget. Cras sit amet ante et dui sodales condimentum sit amet ut odio. Aliquam sollicitudin, sem vel eleifend ornare, turpis risus interdum turpis, non dictum dolor magna auctor urna." };

                // create...
                List<Report> reports = new List<Report>();
                for (int index = 0; index < 50; index++)
                {
                    string title = Randomize(rand, verbs, adjectives, nouns);
                    string description = GetRandom(rand, lipsums);

                    // create...
                    decimal lat = Latitudes[0];
                    decimal lng = Longitude[0];
                    reports.Add(Report.CreateReport(context, context.User, title, description, lat, lng));
                }

                // return.. including fudged validator output...
                output["reports"] = Entity.ToJson<Report>(reports);
                output["reportsExisted"] = false;
            }
            else
                output["reportsExisted"] = true;

            // ok...
            new AjaxValidator().Apply(output);
        }

        private string GetRandom(Random rand, string[] values)
        {
            return values[rand.Next(0, values.Length)];
        }

        private string Randomize(Random rand, params string[][] sets)
        {
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < sets.Length; index++)
            {
                if (index > 0)
                    builder.Append(" ");

                string word = this.GetRandom(rand, sets[index]);
                if (index > 0)
                    word = word.ToLower();

                builder.Append(word);
            }

            return builder.ToString();
        }
    }
}