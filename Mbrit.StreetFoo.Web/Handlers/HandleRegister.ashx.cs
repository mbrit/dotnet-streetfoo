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
            string username = input.GetValueAsString("username");
            string email = input.GetValueAsString("email");
            string password = input.GetValueAsString("password");

            // valid..
            AjaxValidator validator = new AjaxValidator();

            // get...
            User user = User.GetByUsername(api, username);
            if (user == null)
            {
                user = User.CreateUser(api, username, email, password);
                if (user == null)
                    throw new InvalidOperationException("'user' is null.");

                // set...
                output["userId"] = user._id;
            }
            else
                validator.AddError("Username already in use.");

            // set...
            validator.Apply(output);
        }
    }
}