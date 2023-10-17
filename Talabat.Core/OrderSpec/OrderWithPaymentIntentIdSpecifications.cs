using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrdersAggregate;
using Talabat.Core.Specifications;

namespace Talabat.Core.OrderSpec
{
    public class OrderWithPaymentIntentIdSpecifications :BaseSpecification<Order>
    {
        public OrderWithPaymentIntentIdSpecifications(string paymentIntentId)
            :base(O => O.PaymentIntenId == paymentIntentId)
        {
           
        }
    }
}
