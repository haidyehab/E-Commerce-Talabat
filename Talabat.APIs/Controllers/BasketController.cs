using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
   
    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        //use to get or recreate basket
        [HttpGet("{id}")]//Get : api/baskets/id
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return basket is null ?  new CustomerBasket(id) : basket;

        }

        //this do update and create for firstTime

        [HttpPost]//Post : api/baskets
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket=_mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
          var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
            if (createdOrUpdatedBasket is null)
                return BadRequest(new ApiResponse(400));
            return Ok(createdOrUpdatedBasket);
        }

        [HttpDelete]//Delete : api/baskets

        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }

    }
}
