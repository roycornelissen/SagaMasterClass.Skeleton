using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Contracts
{
    using NServiceBus;
    public class OrderAbandoned: IEvent
    {
        public string OrderId
        {
            get;
            set;
        }
    }
}
