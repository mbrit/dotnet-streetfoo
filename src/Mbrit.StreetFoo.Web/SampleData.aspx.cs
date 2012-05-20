using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mbrit.StreetFoo.Web.Handlers;

namespace Mbrit.StreetFoo.Web
{
    public partial class SampleData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // ok...
            this.buttonEnsureSampleData.Click += new EventHandler(buttonEnsureSampleData_Click);
        }

        void buttonEnsureSampleData_Click(object sender, EventArgs e)
        {
            this.labelMessage.Text = string.Empty;

            // get...
            string apiKey = this.textApiKey.Text.Trim();
            if (string.IsNullOrEmpty(apiKey))
            {
                this.labelMessage.Text = "You must enter the API key.";
                return;
            }
            string username = this.textUsername.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                this.labelMessage.Text = "You must enter the username.";
                return;
            }

            // load the api...
            Entities.ApiUser api = Entities.ApiUser.GetOrCreateApiUser(new Guid(apiKey));
            if(api == null)
	            throw new InvalidOperationException("'api' is null.");

            // get the user...
            Entities.User user = Entities.User.GetByUsername(api, username);
            if(user == null)
            {
                this.labelMessage.Text = "The user '{0}' could not be found.";
                return;
            }

            // input...
            JsonData input = new JsonData();
            input["apiKey"] = apiKey;
            input["logonToken"] = Entities.Token.CreateToken(api, user).TheToken;

            // create...       
            HandleEnsureTestReports handler = new HandleEnsureTestReports();
            JsonData output = new JsonData();
            handler.DoRequest(input, output);

            // ok...
            this.labelMessage.Text = output.GetValueAsString("isOk");
        }
    }
}