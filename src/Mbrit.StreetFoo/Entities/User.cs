using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;

namespace Mbrit.StreetFoo.Entities
{
    public class User : ApiBoundEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }

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
            user.SetPassword(password);
            
            // dump...
            using (MongoWrapped db = FooRuntime.GetDatabase())
                db.GetCollection<User>().Insert(user);

            // return...
            return user;
        }

        private void SetPassword(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            if (password.Length == 0)
                throw new ArgumentException("'password' is zero-length.");

            var deriveBytes = new Rfc2898DeriveBytes(password, 20);
            this.PasswordSalt = deriveBytes.Salt;
            this.PasswordHash = deriveBytes.GetBytes(20);  // derive a 20-byte key
        }

        public bool CheckPassword(string password)
        {
            var deriveBytes = new Rfc2898DeriveBytes(password, this.PasswordSalt);

            // get...
            byte[] hash = deriveBytes.GetBytes(20);  // derive a 20-byte key
            return hash.SequenceEqual(this.PasswordHash);
        }
    }
}
