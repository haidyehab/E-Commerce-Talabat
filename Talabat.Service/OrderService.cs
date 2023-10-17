using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrdersAggregate;
using Talabat.Core.OrderSpec;
using Talabat.Core.Repositories;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _productsRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenericRepository<Order> _ordersRepo;

        public OrderService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork
            ,IPaymentService paymentService
            //, IGenericRepository<Product> productsRepo,
            //IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            //IGenericRepository<Order> ordersRepo
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            //_productsRepo = productsRepo;
            //_deliveryMethodRepo = deliveryMethodRepo;
            //_ordersRepo = ordersRepo;
        }
        public async Task<Order?> GreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //1.Get Basket from Baskets Repo
            var basket = await _basketRepository.GetBasketAsync(basketId);
           //2.Get Selected Items at Basket from products Repo
           var orderItems = new List<OrderItem>();
            if(basket?.Items?.Count > 0)//this if basket is not empty
            {
                foreach(var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(product.Id,product.Name,product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered ,product.Price,item.Quantity);
                    orderItems.Add(orderItem);
                }
            }
           //3.Calculate SubTotal
           var subTotal = orderItems.Sum(item => item.Price * item.Quantity);
            //4.GetDelivery Method From DeliveryMethods Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            //5.Creater Order
            var spec = new OrderWithPaymentIntentIdSpecifications(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if(existingOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }
            var order = new Order(buyerEmail, shippingAddress,deliveryMethod,orderItems,subTotal,basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().Add(order);
            //6.Save To Database
             var result = await _unitOfWork.Complete();
            if(result <=0)
                return null;
            return order;
        }

      
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
           
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderSpecifications(orderId, buyerEmail);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliverMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliverMethods;
        }
    }
}
