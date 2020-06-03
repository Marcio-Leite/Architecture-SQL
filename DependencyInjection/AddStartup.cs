using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ProductApp.Service;
using ProductApp.UseCases.AddProduct;
using ProductApp.UseCases.DeleteProduct;
using ProductApp.UseCases.GetProducts;
using ProductApp.UseCases.UpdateProduct;
using ProductApp.UseCasesInterfaces.AddProduct;
using ProductApp.UseCasesInterfaces.DeleteProduct;
using ProductApp.UseCasesInterfaces.GetProducts;
using ProductApp.UseCasesInterfaces.UpdateProduct;
using Repository.Interfaces;
using Repository.Persistence;
using Repository.UnitOfWork;

namespace DependencyInjection
{
    public static class AddStartup
    {
 
        public static void AddProductDependencyInjection(IServiceCollection services)
        {
            services.AddTransient<IAddProduct, AddProduct>();
            services.AddTransient<IGetProducts, GetProducts>();
            services.AddTransient<IUpdateProduct, UpdateProduct>();
            services.AddTransient<IDeleteProduct, DeleteProduct>();
            services.AddTransient<IProductService, ProductApp.Service.ProductService>();
            services.AddScoped<IUnitOfWorkDapper, UnitOfWorkDapper>();
        }
        
        public static void AddSwaggerAndSecurityToApp(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservice Products V1");
            });
            
            app.UseAuthentication();
            app.UseAuthorization();
        }
        
        public static void AddSwaggerAndDependencies(IServiceCollection services, string title, string version, string description, IConfiguration configuration)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = title,
                        Version = version,
                        Description = description
                    });
            });
            
            services.Configure<GzipCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.Optimal;
                })
                .AddResponseCompression(options =>
                {
                    options.Providers.Add<GzipCompressionProvider>();
                    options.EnableForHttps = true;
                });
        }
    }
}