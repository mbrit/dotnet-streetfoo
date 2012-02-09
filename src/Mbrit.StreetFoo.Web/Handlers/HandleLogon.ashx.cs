using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mbrit.StreetFoo.Entities;

namespace Mbrit.StreetFoo.Web.Handlers
{
    /// <summary>
    /// Summary description for HandleLogon
    /// </summary>
    public class HandleLogon : AjaxHandler
    {
        protected override void DoRequest(ApiUser api, JsonData input, JsonData output)
        {
            // get...
            AjaxValidator validator = new AjaxValidator();
            string username = validator.GetRequiredString(input, "username");
            string password = validator.GetRequiredString(input, "password");

            // ok?
            if (validator.IsOk)
            {
                // get...
                User user = User.GetByUsername(api, username);
                if (user != null)
                {
                    // check...
                    if (user.CheckPassword(password))
                    {
                        // create an access token...
                        Token token = Token.CreateToken(api);
                        if (token == null)
                            throw new InvalidOperationException("'token' is null.");

                        // set...
                        output["token"] = token.TheToken;
                    }
                    else
                        validator.AddError("Password is invalid.");
                }
                else
                    validator.AddError("Username is invalid.");
            }

            // set...
            validator.Apply(output);
        }
    }
}