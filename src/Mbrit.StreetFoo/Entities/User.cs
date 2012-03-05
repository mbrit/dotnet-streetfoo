using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Security.Cryptography;

namespace Mbrit.StreetFoo.Entities
{
    public class User : ApiBoundEntity
    {
        [BsonElement(UsernameKey)]
        public string Username { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("passwordSalt")]
        public byte[] PasswordSalt { get; set; }

        [BsonElement("passwordHash")]
        public byte[] PasswordHash { get; set; }

        private const string UsernameKey = "username";

        public static User GetByUsername(IApiUserSource api, string username)
        {
            using (MongoConnection db = FooRuntime.GetDatabase())
            {
                QueryDocument query = new QueryDocument();
                query.AddApiConstraint(api);
                query.Add(UsernameKey, username);

                return db.GetCollection<User>().FindOne(query);
            }
        }

        public static User CreateUser(IApiUserSource api, string username, string email, string password)
        {
            User user = new User();
            user.SetApi(api);
            user.Username = username;
            user.Email = email;
            user.SetPassword(password);
            
            // dump...
            using (MongoConnection db = FooRuntime.GetDatabase())
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

        public static User GetById(IApiUserSource api, ObjectId userId, bool throwIfNotFound)
        {
            using (MongoConnection db = FooRuntime.GetDatabase())
            {
                QueryDocument query = new QueryDocument();
                query.AddApiConstraint(api);
                query.Add(IdKey, userId);

                User user = db.GetCollection<User>().FindOne(query);
                if (user != null)
                    return user;
                else
                {
                    if (throwIfNotFound)
                        throw new InvalidOperationException(string.Format("A user with ID '{0}' was not found.", userId));
                    else
                        return null;
                }
            }
        }

        public IEnumerable<Report> GetReports(IApiUserSource api)
        {
            return Report.GetByUser(api, this);
        }

        public bool HasReports(IApiUserSource api)
        {
            using (MongoConnection db = FooRuntime.GetDatabase())
            {
                QueryDocument query = new QueryDocument();
                query.AddApiConstraint(api);
                query.Add("ownerUserId", this._id);

                Report report = db.GetCollection<Report>().FindOne(query);
                return report != null;
            }
        }
    }
}
