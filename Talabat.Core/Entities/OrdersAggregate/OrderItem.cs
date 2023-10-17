using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.OrdersAggregate
{
    public class OrderItem :BaseEntity
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItemOrdered product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrdered Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        //public Order Order { get; set; } //i do not need to do this because EF will do this
    }
}
