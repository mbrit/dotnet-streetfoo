using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mbrit.StreetFoo.Entities
{
    public class User : ApiBoundEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int PasswordSalt { get; set; }
        public string PasswordHash { get; set; }

        public static User GetByUsername(ApiUser api, string username)
        {
            using (MongoWrapped db = FooRuntime.GetDatabase())
            {
                QueryDocument query = new QueryDocument();
                query.Add("ApiKey", api.ApiKey);
                query.Add("Username", username);

                return db.GetCollection<User>().FindOne(query);
            }
        }

        public static User CreateUser(ApiUser api, string username, string email, string password)
        {
            User user = new User();
            user.SetApi(api);
            user.Username = username;
            user.Email = email;
            
            // dump...
            using (MongoWrapped db = FooRuntime.GetDatabase())
                db.GetCollection<User>().Insert(user);

            // return...
            return user;
        }
    }
}
