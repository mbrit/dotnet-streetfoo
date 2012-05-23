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
        protected override void DoRequest(AjaxContext context, JsonData input, JsonData output)
        {
            // get...
            AjaxValidator validator = new AjaxValidator();
            string username = validator.GetRequiredString(input, "username");
            string email = validator.GetRequiredString(input, "email");
            string password = validator.GetRequiredString(input, "password");
            string confirm = validator.GetRequiredString(input, "confirm");

            // check...
            if (validator.IsOk)
            {
                if (!(string.IsNullOrEmpty(password)) && password != confirm)
                    validator.AddError("Passwords do not match.");
            }

            // ok?
            if (validator.IsOk)
            {
                // get...
                User user = User.GetByUsername(context, username);
                if (user == null)
                {
                    user = User.CreateUser(context, username, email, password);
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