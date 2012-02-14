using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mbrit.StreetFoo.Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.GetNewCode();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.buttonRefresh.Click += new EventHandler(buttonRefresh_Click);
        }

        void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.GetNewCode();
        }

        private void GetNewCode()
        {
            this.textGuid.Text = Guid.NewGuid().ToString();
        }
    }
}