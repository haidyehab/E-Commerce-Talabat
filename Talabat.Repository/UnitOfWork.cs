using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrdersAggregate;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        //private Dictionary<string, object> _repositories;
        //private Dictionary<string, GenericRepository<BaseEntity>> _repositories;
        private Hashtable _repositories;

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;//product
            if(!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);//this is value

                _repositories.Add(type,repository);
            }
            return _repositories[type] as IGenericRepository<TEntity>;//in hashtable will return object so will do ExiplicitCasting
               
        }

        public async Task<int> Complete()
        
        =>   await _dbContext.SaveChangesAsync();
        

        public async ValueTask DisposeAsync()
       => await _dbContext.DisposeAsync();

       
    }
}
