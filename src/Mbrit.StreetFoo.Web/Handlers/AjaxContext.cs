using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mbrit.StreetFoo.Entities;

namespace Mbrit.StreetFoo.Web
{
    public class AjaxContext : IApiUserSource
    {
        public ApiUser ApiUser { get; private set; }
        private Token Token { get; set; }
        private User _user;

        internal AjaxContext(JsonData input)
        {
            // get the api out...
            string apiKey = input.GetValueAsString("apiKey");
            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("The 'apiKey' value was not specified in the request.");
            this.ApiUser = ApiUser.GetOrCreateApiUser(new Guid(apiKey));
            if(ApiUser == null)
	            throw new InvalidOperationException("'ApiUser' is null.");

            // do we have a logon token?
            string asString = input.GetValueAsString("logonToken");
            if (!(string.IsNullOrEmpty(asString)))
            {
                Token token = Token.GetByToken(this.ApiUser, asString);
                if (token == null)
                    throw new InvalidOperationException(string.Format("A token with ID '{0}' was not found.", asString));

                // update...
                token.UpdateExpiration();

                // set...
                this.Token = token;
            }
        }

        public User User
        {
            get
            {
                if (_user == null)
                {
                    if (this.Token == null)
                        throw new InvalidOperationException("A logon token ('logonToken') was not supplied.");

                    // get...
                    _user = User.GetById(this.ApiUser, this.Token.UserId, true);
                    if (_user == null)
                        throw new InvalidOperationException("'_user' is null.");
                }
                return _user;
            }
        }
    }
}