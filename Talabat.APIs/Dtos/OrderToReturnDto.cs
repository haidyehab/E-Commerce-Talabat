﻿using Talabat.Core.Entities.OrdersAggregate;

namespace Talabat.APIs.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; } 
        public Address ShippingAddress { get; set; }
      
        //public DeliveryMethod DeliveryMethod { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemDto> Items { get; set; }

        public decimal Subtotal { get; set; }
        public string PaymentIntenId { get; set; } 
        public decimal Total { get; set; }


    }
}
