using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Mbrit.StreetFoo.Entities
{
    public abstract class Entity
    {
        public ObjectId _id { get; set; }
    }
}
