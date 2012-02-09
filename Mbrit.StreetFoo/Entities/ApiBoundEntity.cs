using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mbrit.StreetFoo.Entities
{
    public abstract class ApiBoundEntity : Entity
    {
        public string ApiKey { get; set; }

        public void SetApi(ApiUser api)
        {
            if (api == null)
                throw new ArgumentNullException("api");
            this.ApiKey = api.ApiKey;
        }
    }
}
