using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.OrdersAggregate;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
  
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _whSecret = "whsec_cd552525b7e67ce60e983254fa7c7c3126452787c3849c7752683ff10676102e";
        public PaymentsController(IPaymentService paymentService,ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }


        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]//post : /api/Payments?id=basketId
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatedPaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null)
                return BadRequest(new ApiResponse(400, "A problem with your Basket"));
            return Ok(basket);
        }


        [HttpPost("webhook")]//post :/api/payments/webhook
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
           
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _whSecret);
                var paymentIntent = (PaymentIntent) stripeEvent.Data.Object;

                Order order;

                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                      order=  await _paymentService.UpdatedPaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                        _logger.LogInformation("Payment is Succeeded",paymentIntent.Id);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                       order= await _paymentService.UpdatedPaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                        _logger.LogInformation("Payment is Failed", paymentIntent.Id);
                        break;
                }
                return Ok();
            
        }


    }
}
