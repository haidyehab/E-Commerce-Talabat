using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)//ask clr create object from DbContext implicitly
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
                return (IReadOnlyList<T>)await _dbContext.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();
            //return (IReadOnlyList<T>)await _dbContext.Products..OrderBy(P => P.Name).Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();
            return await _dbContext.Set<T>().ToListAsync();
        }

       
        public async Task<T> GetByIdAsync(int id)
        {
            _dbContext.Products.Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType);
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        //count
        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }
        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>() ,spec);
        }

        public async Task Add(T entity)
        
        =>   await _dbContext.Set<T>().AddAsync(entity);
        

        public void Update(T entity)
        => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
        => _dbContext.Set<T>().Remove(entity);
    }
}
