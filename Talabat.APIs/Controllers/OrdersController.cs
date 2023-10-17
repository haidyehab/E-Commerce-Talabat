using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.OrdersAggregate;
using Talabat.Core.OrderSpec;
using Talabat.Core.Services;
using Talabat.Repository.Data.Migrations;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController :ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost]//Post : /api/Orders
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var address = _mapper.Map<AddressDto, Address>(orderDto.shipToAddress);
          var order=  await _orderService.GreateOrderAsync(buyerEmail,orderDto.BasketId,orderDto.DeliveryMethodId,address);
            if (order is null)
                return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Order,OrderToReturnDto>(order));
        }

        [HttpGet]//Get :/api/Orders
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders =await _orderService.GetOrdersForUserAsync(buyerEmail);
            return Ok(_mapper.Map<IReadOnlyList<Order>,IReadOnlyList< OrderToReturnDto>>(orders));
        }

        //get one order for user
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]//Get :/api/orders/1
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUserAsync(id, buyerEmail);
            if (order is null)
                return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }

        //DeliverMethods
        [HttpGet("deliveryMethods")]//Get : /api/Orders
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliverMethods()
        {
            var deliverMethods = await _orderService.GetDeliveryMethodsAsync();
            return Ok(deliverMethods);
        }


    }
}
