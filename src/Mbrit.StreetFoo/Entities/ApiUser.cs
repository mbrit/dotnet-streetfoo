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

        public static ApiUser GetOrCreateApiUser(Guid apiKey)
        {
            using (MongoConnection db = FooRuntime.GetDatabase())
            {
                try
                {
                    MongoCollection<ApiUser> users = db.GetCollection<ApiUser>();

                    // create...
                    ApiUser user = new ApiUser();
                    user.ApiKey = apiKey.ToString();
                    user.CreatedUtc = DateTime.UtcNow;
                    users.Insert(user);

                    // return..
                    return user;
                }
                catch (Exception ex)
                {
                    throw db.WrapError(ex);
                }
            }
        }

        public static ApiUser CreateMockApiUser()
        {
            ApiUser api = new ApiUser();
            api.ApiKey = Guid.NewGuid().ToString();

            return api;
        }
    }
}
