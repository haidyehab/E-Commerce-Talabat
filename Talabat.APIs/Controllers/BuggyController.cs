using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
   
    public class BuggyController : ApiBaseController
    {
        //this controller to display form of Error
        private readonly StoreContext _dbContext;

        public BuggyController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        //1.NotFound => this occurs when it no has any things with id
        [HttpGet("notfound")]//Get :api/Buggy/notfound 404

        public ActionResult GetNotFoundRequest()
        {
            var product = _dbContext.Products.Find(100);
            if (product is null)
                return NotFound(new ApiResponse(404));

            return Ok(product);
        }

        //2.ServerError
        [HttpGet("servererror")]//Get :api/Buggy/servererror

        public ActionResult GetServerError()
        {
            var product = _dbContext.Products.Find(100);//I sure that product = null because I no have 100 product
            var productToReturn = product.ToString();//will threw Exception [NullReferenceException]

            return Ok(productToReturn);
        }

        //3.BadRequest
        [HttpGet("badrequest")]//Get :api/Buggy/badrequest

        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        //4.validation Error this is part of BadRequest
        [HttpGet("badrequest/{id}")] //Get :api/Buggy/badrequest/5
                                       // but I send that //Get :api/Buggy/badrequest/five 
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }

        //5.Not Found Endpoint //Get :api/Hamada
    }
}
