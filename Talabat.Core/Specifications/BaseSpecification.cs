using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get ; set ; }
        public List<Expression<Func<T, object>>> Includes { get; set;} = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get ; set ; }
        public Expression<Func<T, object>> OrderByDescending { get ; set ;}
        public int Skip { get ; set ; }
        public int Take { get ; set ; }
        public bool IsPaginationEnable { get ; set ; }

        public BaseSpecification()
        {
         
        }

        public BaseSpecification(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression; //P => (P.ProductBrandId == 1 && true)
                                           //O => O.BuyerEmail =="ahmed.nasr@linksev.com
        }

        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression; //P => P.Name
        }

        public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;//P => P.Name
                                                      //O => O.orderDate
        }


        public void ApplyPagination(int skip , int take)
        {
            IsPaginationEnable = true;
            Skip = skip;
            Take = take;
        }
    }
}
