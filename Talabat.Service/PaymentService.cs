using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrdersAggregate;
using Talabat.Core.OrderSpec;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(
            IConfiguration configuration,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork
            )
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            var basket =await _basketRepository.GetBasketAsync( basketId);
            if (basket is null)
                return null;
            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliverMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                basket.ShippingCost = deliverMethod.Cost;
                shippingPrice = deliverMethod.Cost;
            }

            if(basket?.Items?.Count > 0)
            {
                foreach(var item in basket.Items) 
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if(item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;
           
            if(string.IsNullOrEmpty(basket.PaymentIntentId))//Create paymentIntent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount =(long) basket.Items.Sum(item => item .Price *item.Quantity *100) + (long) shippingPrice *100,
                    Currency ="usd",
                    PaymentMethodTypes = new List<string>() { "card"}
                };
                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else//update paymentInten
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100,
                };

                await service.UpdateAsync(basket.PaymentIntentId,options);
            }

            await _basketRepository.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdatedPaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded)
        {
            var spec = new OrderWithPaymentIntentIdSpecifications(paymentIntentId);
            var order =await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (isSucceeded)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.Complete();
            return order;

        }
    }
}
