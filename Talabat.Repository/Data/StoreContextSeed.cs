using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrdersAggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            //brands
            if(!dbContext.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {
                        await dbContext.Set<ProductBrand>().AddAsync(brand);
                    }
                    await dbContext.SaveChangesAsync();
                }}

            //types
            if (!dbContext.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                if (types?.Count > 0)
                {
                    foreach (var type in types)
                    {
            
                     await dbContext.Set<ProductType>().AddAsync(type);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }


            //products
            if (!dbContext.Products.Any())
            {
                var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {

                        await dbContext.Set<Product>().AddAsync(product);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            //DeliveryMethod
            if (!dbContext.DeliveryMethods.Any())
            {
                var MethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var Methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(MethodsData);
                if (Methods?.Count > 0)
                {
                    foreach (var method in Methods)
                    {

                        await dbContext.Set<DeliveryMethod>().AddAsync(method);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }



        }
    }
}
