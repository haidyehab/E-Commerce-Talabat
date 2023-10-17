using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrdersAggregate;
using Talabat.Core.Specifications;

namespace Talabat.Core.OrderSpec
{
    public class OrderSpecifications : BaseSpecification<Order>
    {
        //this use to get Orders
        public OrderSpecifications(string email)
         :base(O => O.BuyerEmail == email)//will chain in ctor which take criteria
        {
            Includes.Add(O => O.DeliveryMethod);//one
            Includes.Add(O => O.Items);//many

            AddOrderByDesc(O => O.OrderDate);
        }
        //this use to get specific Order
        public OrderSpecifications(int id,string email)
        : base(O => O.BuyerEmail == email && O .Id == id)//will chain in ctor which take criteria
        {
            Includes.Add(O => O.DeliveryMethod);//one
            Includes.Add(O => O.Items);//many

          
        }
    }
}
