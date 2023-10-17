using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BrandsWithSpecification :BaseSpecification<ProductBrand>
    {
        public BrandsWithSpecification(BrandAndTypeSpecParams specParams)
        {
            AddOrderBy(B => B.Name);
            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }
    }
}
