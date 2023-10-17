using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;
       

        public CachedAttribute(int timeToLiveInSeconds )
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
           
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCaheKeyFromRequest(context.HttpContext.Request);
            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);
            if(!string.IsNullOrEmpty(cachedResponse) )
            {
                var contentResult = new ContentResult()
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

         var executedEndpointContext =  await next.Invoke();//will execute the Endpoint
            if(executedEndpointContext.Result is ObjectResult okObjectResult)
            {
                await cacheService.CacheResponseAsync(cacheKey,okObjectResult.Value,TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GenerateCaheKeyFromRequest(HttpRequest request)
        {
            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(request.Path);// /api/products
            //pageIndex =1;
            //pageSize =5;
            //sort=name;
            foreach( var (key,value) in request.Query.OrderBy(x => x.Key) )
            {
                KeyBuilder.Append($"|{key}-{value}");
                //1. / api / products |pageIndex =1
                //2. / api / products |pageIndex =1 |pageSize =5
                //3. / api / products |pageIndex =1 |pageSize =5 |sort=name
            }
            return KeyBuilder.ToString();
        }
    }
}
