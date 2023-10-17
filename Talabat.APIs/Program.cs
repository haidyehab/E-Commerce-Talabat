using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MidleWares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region Configure Service

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServices();


            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                

			});

            builder.Services.AddDbContext<AppicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });


            builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            builder.Services.AddApplicationServices();

            
            builder.Services.AddIdentityServices(builder.Configuration);
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("myPolicy", options =>
            //    {
            //        options.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontUrl"]);
            //    });
            //});
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("myPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            #endregion

            var app = builder.Build();

           using var scope = app.Services.CreateScope();//Explicilty
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            
            try
            {
                var dbContext = services.GetRequiredService<StoreContext>();//Ask CLR creating Object from DbContext Expliclty
                await dbContext.Database.MigrateAsync();//update-database

                var identityDbContext = services.GetRequiredService<AppicationIdentityDbContext>();
                await identityDbContext.Database.MigrateAsync();//update-database

                await StoreContextSeed.SeedAsync(dbContext);

                var userManager = services.GetRequiredService<UserManager<AppUser>>(); //cl ask create object from usermanager
                await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
            }
            catch (Exception ex)
            {

             var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex,"an error occured during apply the migration");
            }


            app.UseMiddleware<ExceptionMiddlewares>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleware();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors("myPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}