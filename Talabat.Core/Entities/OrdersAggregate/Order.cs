using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.OrdersAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subtotal,string PaymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            PaymentIntenId = PaymentIntentId;
            Items = items;
            Subtotal = subtotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }
        
        public DeliveryMethod DeliveryMethod { get; set; }//navigation property one

        public ICollection<OrderItem> Items { get; set;} = new HashSet<OrderItem>();//navigation property many

        public decimal Subtotal { get; set; }
       
        public decimal GetTotal()
         => Subtotal + DeliveryMethod.Cost;

        public string PaymentIntenId { get; set; }
    }
}
