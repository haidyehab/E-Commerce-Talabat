using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
   
    public class ProductsController : ApiBaseController
    {
        //private readonly IGenericRepository<Product> _productsRepo;
        //private readonly IGenericRepository<ProductBrand> _brandsRepo;
        //private readonly IGenericRepository<ProductType> _typesRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(
             IMapper mapper,
             IUnitOfWork unitOfWork
           // IGenericRepository<Product> productsRepo ,
           //IGenericRepository<ProductBrand> brandsRepo,
           // IGenericRepository<ProductType> typesRepo
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            //_productsRepo = productsRepo;
            //_brandsRepo = brandsRepo;
            //_typesRepo = typesRepo;
        }


        [CachedAttribute(600)]
        [HttpGet]//Get\api\Products
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            /*var spec = new BaseSpecification<Product>();*/ //here I use empty Constructor
            // before excute this ctor it will chain for empty parameters ctor of parent [BaseSpecification]
            var spec = new ProdcutWithBrandAndTypeSpecifications(specParams); //here I use empty Constructor
            var products =await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var countSpec = new ProductWithFilterationForCountSpecification(specParams);
            var count= await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);
             return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex,specParams.PageSize,count,data));
        }

        [CachedAttribute(600)]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]//Get\api\Products\id
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProdcutWithBrandAndTypeSpecifications(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            if (product is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }


        //brands and type with Paginations
        //[HttpGet("brands")]//Get :api/Products/brands

        //public async Task<ActionResult<Pagination<ProductBrand>>> GetBrands([FromQuery] BrandAndTypeSpecParams specParams)
        //{
        //    var spec = new BrandsWithSpecification(specParams);
        //    var brands = await _unitOfWork.Repository<ProductBrand>().GetAllWithSpecAsync(spec);
        //    var data = brands;
        //    var countSpec = new BrandsCountWithSpec();
        //    var count = await _unitOfWork.Repository<ProductBrand>().GetCountWithSpecAsync(countSpec);
        //    return Ok(new Pagination<ProductBrand>(specParams.PageIndex, specParams.PageSize, count, data));
        //}

        //[HttpGet("types")]//Get :api/Products/types

        //public async Task<ActionResult<Pagination<ProductType>>> GetTypes([FromQuery] BrandAndTypeSpecParams specParams)
        //{
        //    var spec = new TypeWithSpecification(specParams);
        //    var types = await _unitOfWork.Repository<ProductType>().GetAllWithSpecAsync(spec);
        //    var data = types;
        //    var countSpec = new TypeCountWithSpec();
        //    var count = await _unitOfWork.Repository<ProductType>().GetCountWithSpecAsync(countSpec);
        //    return Ok(new Pagination<ProductType>(specParams.PageIndex, specParams.PageSize, count, data));
        //}
        [CachedAttribute(600)]
        [HttpGet("brands")]//Get :api/Products/brands

        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }
        [CachedAttribute(600)]
        [HttpGet("types")]//Get :api/Products/types

        public async Task<ActionResult<IEnumerable<ProductType>>> GetTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }


    }
}
