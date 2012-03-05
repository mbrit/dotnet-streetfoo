using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.IO;
using System.IO;
using System.Web.Script.Serialization;
using System.Reflection;

namespace Mbrit.StreetFoo.Entities
{
    public abstract class Entity
    {
        // mbr - 2012-03-05 - don't be tempted to rename this - seems to break it...
        [BsonElement("_id")]
        public ObjectId _id { get; set; }

        internal const string IdKey = "_id";

        private static bool MappingsDone { get; set; }

        public static string ToJson<T>(IEnumerable<T> items)
            where T : Entity
        {
            EnsureMappings();

            List<object> theItems = new List<object>();
            foreach (T item in items)
                theItems.Add(item.ToDictionary());

            // return...
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(theItems);
        }

        public static string ToJson<T>(T item)
        {
            throw new NotImplementedException("This operation has not been implemented.");
        }

        private static void EnsureMappings()
        {
            // mbr - 2012-03-05 - done lazily during use... was here when working how how to do the serialization...
            //if (MappingsDone)
            //    return;

            //// go...
            //BsonClassMap.RegisterClassMap<ApiUser>();
            //BsonClassMap.RegisterClassMap<User>();
            //BsonClassMap.RegisterClassMap<Token>();
            //BsonClassMap.RegisterClassMap<Report>();

            //// set...
            //MappingsDone = true;
        }

        private Dictionary<string, object> ToDictionary()
        {
            // walk...
            Dictionary<string, object> values = new Dictionary<string, object>();
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                BsonElementAttribute[] attrs = (BsonElementAttribute[])prop.GetCustomAttributes(typeof(BsonElementAttribute), false);
                if (attrs.Length > 0)
                {
                    object value = prop.GetValue(this, null);
                    if (value is ObjectId)
                        value = value.ToString();

                    // set...
                    values[attrs[0].ElementName] = value;
                }
            }

            // return...
            return values;
        }

        public string IdAsString
        {
            get
            {
                if (_id != null)
                    return _id.ToString();
                else
                    return null;
            }
        }
    }
}
