using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    //TEntity => T
    public static class SpecificationEvaluator<TEntity> where TEntity :BaseEntity
    {
        // _dbContext.Products.Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType)
        // IQueryable<TEntity> inputQuery => _dbContext.Products
        //ISpecification<TEntity> spec => criteria and includes
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecification<TEntity> spec)
        {
            var query = inputQuery;//inputQuery =>_dbContext.Set<Product>(),_dbContext.orders
            if (spec.Criteria is not null) //Criteria =>P => P.Id ==1
                                            // in sortFilter ===> P => (P.ProductBrandId == 1 && true)
           query = query.Where(spec.Criteria);// query=_dbContext.Set<Product>().Where(P => P.Id ==1)
                                              // in sortFilter ===> query=_dbContext.Set<Product>().Where(P => (P.ProductBrandId == 1 && true))   
                                              //query=query=_dbContext.Set<orderst>().Where(O => O.Email == email) 



            //sort
            //query = _dbContext.Set<Product>();
            if (spec.OrderBy is not null)// P => P.Price ,default :P => P.Name
                query = query.OrderBy(spec.OrderBy);//query = _dbContext.Set<Product>().orderBy( P => P.Price);
                                                    // in sortFilter ==>query = _dbContext.Set<Product>()
                                                    // .Where(P => (P.ProductBrandId == 1) && true))
                                                    // .orderBy( P => P.Price);

           if (spec.OrderByDescending is not null) // P => P.Price
                query = query.OrderByDescending(spec.OrderByDescending);//query = _dbContext.Set<Product>().orderByDescending( P => P.Price);
                                                                        //query=query=_dbContext.Set<orderst>().Where(O => O.Email == email) 
              //pagination                                             //.orderByDescending( O => O.orderDate);

            if (spec.IsPaginationEnable)
                query = query.Skip(spec.Skip).Take(spec.Take);

            //query = _dbContext.Set<Product>().orderBy( P => P.Price).Skip(10).Take(10);
            // in sortFilter ==>query = _dbContext.Set<Product>()
            // .Where(P => (P.ProductBrandId == 1) && true))
            // .orderBy( P => P.Price).Skip(10).Take(10);




            //Includes
            //1. P => P.ProductBrand
            //2. P => P.ProductType

            //query = _dbContext.Set<Product>().orderBy( P => P.Price).Include(P => P.ProductBrand).Include(P => P.ProductType);
            //query = _dbContext.Set<Product>().orderByDescending( P => P.Price).Include(P => P.ProductBrand).Include(P => P.ProductType);;


            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            //step one
            //always query in first step is currentQuery
            //1. query = _dbContext.Set<Product>().Where(P => P.Id == 1) => currentQuery
            //Include(P => P.ProductBrand)=>includeExpression
            //2.query = _dbContext.Set<Product>().Where(P => P.Id == 1).Include(P => P.ProductBrand)
            //step two
            //1. query = _dbContext.Set<Product>().Where(P => P.Id == 1).Include(P => P.ProductBrand) =>currentQuery
            //2.Include(P => P.ProductType) =>includeExpression
            //finally => query = _dbContext.Set<Product>().Where(P => P.Id == 1).Include(P => P.ProductBrand).Include(P => P.ProductType)


            //query = _dbContext.Set<Product>().orderBy( P => P.Name).Include(P => P.ProductBrand).Include(P => P.ProductType);

            // in FinallysortFilter ==>query = _dbContext.Set<Product>()
            // .Where(P => (P.ProductBrandId == 1 && true))
            // .orderBy( P => P.Price).Include(P => P.ProductBrand).Include(P => P.ProductType);;

            //Pagination

            //1.query = _dbContext.Set<Product>().orderBy( P => P.Price).Skip(10).Take(10).orderBy( P => P.Name)
            //.Include(P => P.ProductBrand).Include(P => P.ProductType);;

            // 2.query = _dbContext.Set<Product>()
            // .Where(P => (P.ProductBrandId == 1) && true))
            // .orderBy( P => P.Price).Skip(10).Take(10).orderBy( P => P.Name).
            // Include(P => P.ProductBrand).Include(P => P.ProductType);;

            //query=query=_dbContext.Set<orderst>().Where(O => O.Email == email) 
            //.orderByDescending( O => O.orderDate). Include(O => O.DeliveryMethod).Include(O => O.ProductType);

            return query;
        }
    }
}
