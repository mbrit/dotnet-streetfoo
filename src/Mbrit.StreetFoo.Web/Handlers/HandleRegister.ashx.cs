using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mbrit.StreetFoo.Entities;

namespace Mbrit.StreetFoo.Web.Handlers
{
    /// <summary>
    /// Summary description for HandleRegister
    /// </summary>
    public class HandleRegister : AjaxHandler
    {
        protected override void DoRequest(ApiUser api, JsonData input, JsonData output)
        {
            if(api == null)
	            throw new ArgumentNullException("api");

            // get...
            AjaxValidator validator = new AjaxValidator();
            string username = validator.GetRequiredString(input, "username");
            string email = validator.GetRequiredString(input, "email");
            string password = validator.GetRequiredString(input, "password");

            // ok?
            if (validator.IsOk)
            {
                // get...
                User user = User.GetByUsername(api, username);
                if (user == null)
                {
                    user = User.CreateUser(api, username, email, password);
                    if (user == null)
                        throw new InvalidOperationException("'user' is null.");

                    // set...
                    output["userId"] = user._id.ToString();
                }
                else
                    validator.AddError("Username already in use.");
            }

            // set...
            validator.Apply(output);
        }
    }
}