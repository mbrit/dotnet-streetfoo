using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using Mbrit.StreetFoo.Entities;

namespace Mbrit.StreetFoo
{
    public static class QueryDocumentExtender
    {
        public static void AddApiConstraint(this QueryDocument doc, IApiUserSource source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (source.ApiUser == null)
                throw new ArgumentNullException("source.ApiUser");

            // set...
            doc.Add(ApiUser.ApiKeyKey, source.ApiUser.ApiKey);
        }
    }
}
