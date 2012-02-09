using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace Mbrit.StreetFoo
{
    public static class MongoCollectionExtender
    {
        public static void Insert2(this MongoCollection items)
        {
            try
            {
                items.Insert(
            }
            catch (Exception ex)
            {
            }
        }
    }
}
