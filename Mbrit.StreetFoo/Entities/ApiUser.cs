using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace Mbrit.StreetFoo.Entities
{
    public class ApiUser : Entity
    {
        public string ApiKey { get; set; }
        public DateTime CreatedUtc { get; set; }

        public static ApiUser GetOrCreateApiUser(string apiKey)
        {
            if (apiKey == null)
                throw new ArgumentNullException("apiKey");
            if (apiKey.Length == 0)
                throw new ArgumentException("'apiKey' is zero-length.");

            using (MongoWrapped db = FooRuntime.GetDatabase())
            {
                MongoCollection<ApiUser> users = db.GetCollection<ApiUser>();

                // create...
                ApiUser user = new ApiUser();
                user.ApiKey = apiKey;
                user.CreatedUtc = DateTime.UtcNow;
                users.Insert(user);

                // return..
                return user;
            }
        }
    }
}
