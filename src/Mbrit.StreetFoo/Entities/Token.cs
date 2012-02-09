using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;

namespace Mbrit.StreetFoo.Entities
{
    public class Token : ApiBoundEntity
    {
        public Guid TheToken { get; set; }

        public static Token CreateToken(ApiUser api)
        {
            Token item = new Token();
            item.SetApi(api);
            item.TheToken = Guid.NewGuid();

            // save...
            using (MongoWrapped db = FooRuntime.GetDatabase())
                db.GetCollection<Token>().Insert(item);

            // return...
            return item;
        }
    }
}
