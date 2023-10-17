using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrdersAggregate;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        //these signature property For do order
        Task<Order?> GreateOrderAsync(string buyerEmail ,string basketId,int deliveryMethodId,Address shippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        Task<Order> GetOrderByIdForUserAsync(int orderId , string buyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
