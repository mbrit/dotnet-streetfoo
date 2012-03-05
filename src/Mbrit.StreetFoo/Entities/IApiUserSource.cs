using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mbrit.StreetFoo.Entities
{
    public interface IApiUserSource 
    {
        ApiUser ApiUser
        {
            get;
        }
    }
}
