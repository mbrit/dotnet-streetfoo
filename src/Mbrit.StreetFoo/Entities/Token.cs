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
    public class Token : ApiBoundEntity
    {
        [BsonElement(TheTokenKey)]
        public Guid TheToken { get; set; }

        [BsonElement("userId")]
        public ObjectId UserId { get; set; }

        [BsonElement("expiration")]
        public DateTime Expiration { get; set; }

        private const string TheTokenKey = "token";

        private const int ExpirationHours = 72;

        public static Token CreateToken(IApiUserSource api, User user)
        {
            Token item = new Token();
            item.SetApi(api);
            item.TheToken = Guid.NewGuid();
            item.UserId = user._id;
            item.UpdateExpiration(false);

            // save...
            using (MongoConnection db = FooRuntime.GetDatabase())
                db.GetCollection<Token>().Insert(item);

            // return...
            return item;
        }

        public void UpdateExpiration()
        {
            this.UpdateExpiration(true);
        }

        private void UpdateExpiration(bool save)
        {
            this.Expiration = DateTime.Now.AddHours(ExpirationHours);

            // save...
            if (save)
                this.SaveChanges();
        }

        public void SaveChanges()
        {
            // tbd...
        }

        public static Token GetByToken(IApiUserSource api, string token)
        {
            return GetByToken(api, new Guid(token));
        }

        public static Token GetByToken(IApiUserSource api, Guid token)
        {
            using (MongoConnection db = FooRuntime.GetDatabase())
            {
                QueryDocument query = new QueryDocument();
                query.AddApiConstraint(api);
                query.Add(TheTokenKey, token);

                return db.GetCollection<Token>().FindOne(query);
            }
        }
    }
}
