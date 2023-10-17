using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProdcutWithBrandAndTypeSpecifications :BaseSpecification<Product>
    {
        //this Constructor is used for Get All Product
        //empty Constructor
        //here criateria is null and includes are  Includes.Add(P => P.ProductBrand) ,Includes.Add(P => P.ProductType)
        public ProdcutWithBrandAndTypeSpecifications(ProductSpecParams specParams)// before excute this ctor it will chain for empty parameters  ctor of parent [BaseSpecification]
         :base(P => //this condition will excute in creteria
           (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search))&&
           (!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId.Value) &&
           (!specParams.TypeId.HasValue ||  P.ProductTypeId == specParams.TypeId.Value)
         )
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            AddOrderBy(P => P.Name);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                      //  OrderBy = P => P.Price;
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                       // OrderByDescending = P => P.Price
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }

            //totalProduct =100
            //pageSize  =10
            //PageIndex =3
            ApplyPagination(specParams.PageSize * (specParams.PageIndex -1),specParams.PageSize);
        }
        //this Constructor is used for Get a specific Product
        // before excute this ctor it will chain for ctor of parent [BaseSpecification]
        public ProdcutWithBrandAndTypeSpecifications(int id):base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }

    }
}
