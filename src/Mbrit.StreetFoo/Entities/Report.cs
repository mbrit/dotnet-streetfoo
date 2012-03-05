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
    public class Report : ApiBoundEntity
    {
        [BsonElement("ownerUserId")]
        public ObjectId OwnerUserId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("latitude")]
        public decimal Latitude { get; set; }

        [BsonElement("longitude")]
        public decimal Longitude { get; set; }

        public static IEnumerable<Report> GetAll(IApiUserSource api)
        {
            using (MongoConnection db = FooRuntime.GetDatabase())
            {
                QueryDocument query = new QueryDocument();
                query.AddApiConstraint(api);

                return db.GetCollection<Report>().Find(query);
            }
        }

        public static IEnumerable<Report> GetByUser(IApiUserSource api, User user)
        {
            using (MongoConnection db = FooRuntime.GetDatabase())
            {
                QueryDocument query = new QueryDocument();
                query.AddApiConstraint(api);
                query.Add("ownerUserId", user._id);

                return db.GetCollection<Report>().Find(query);
            }
        }

        public static Report CreateReport(IApiUserSource api, User owner, string title, string description, decimal latitude, decimal longitude)
        {
            Report report = new Report();
            report.SetApi(api);
            report.OwnerUserId = owner._id;
            report.Title = title;
            report.Description = description;
            report.Latitude = latitude;
            report.Longitude = longitude;
            
            // dump...
            using (MongoConnection db = FooRuntime.GetDatabase())
                db.GetCollection<Report>().Insert(report);

            // return...
            return report;
        }

        public void DeleteReport(IApiUserSource api)
        {
            using (MongoConnection db = FooRuntime.GetDatabase())
            {
                QueryDocument query = new QueryDocument();
                query.AddApiConstraint(api);
                query.Add("id", this._id);

                db.GetCollection<Report>().Remove(query);
            }
        }
    }
}
