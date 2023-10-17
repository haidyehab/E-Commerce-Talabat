using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrdersAggregate;
using Talabat.Repository.Data.Config;

namespace Talabat.Repository.Data
{
    public class StoreContext :DbContext //do context Repository will be here more than repositoryLayer
    {
        public StoreContext(DbContextOptions<StoreContext> options):base(options)
        {
         
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //fluent api

            //modelBuilder.ApplyConfiguration(new ProductConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductBrandConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductTypeConfigurations());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
    }
}
