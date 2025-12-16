using Application.Interfaces;
using Application.Mappers;
using Application.Services;
using Domain.Interfaces;
using InventorySystemAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Inventory.Tests.Infrastructure
{
    public abstract class TestBase : IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;

        protected HttpClient HttpClient { get; }

        protected IInventoryRepository inventoryRepositoryMock { get; }
        protected IProductCategoryRepository productCategoryRepositoryMock { get; }

        protected IProductRepository productRepositoryMock { get; } 

        protected IProductSupplierRepository productSupplierRepositoryMock { get; }

        protected ISupplierRepository supplierRepositoryMock { get; }

        protected TestBase()
        {
            inventoryRepositoryMock = Substitute.For<IInventoryRepository>();
            productCategoryRepositoryMock = Substitute.For<IProductCategoryRepository>();
            productRepositoryMock = Substitute.For<IProductRepository>();
            productSupplierRepositoryMock = Substitute.For<IProductSupplierRepository>();
            supplierRepositoryMock = Substitute.For<ISupplierRepository>();

            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Testing");

                    builder.ConfigureServices(services =>
                    {
                        // ------------------------------------------------
                        // REMOVE mocked AutoMapper
                        // USE REAL AutoMapper configuration
                        // ------------------------------------------------
                        services.AddAutoMapper(typeof(Program));
                        services.AddAutoMapper(typeof(ProductProfile).Assembly);

                        // -----------------------------
                        // APPLICATION SERVICES (REAL)
                        // -----------------------------
                        services.AddScoped<IInventoryService, InventoryService>();
                        services.AddScoped<IProductCategoryService, ProductCategoryService>();
                        services.AddScoped<IProductService, ProductService>();
                        services.AddScoped<IProductSupplierService, ProductSupplierService>();

                        // Replace repository only
                        services.AddScoped(_ => inventoryRepositoryMock);
                        services.AddScoped(_ => productCategoryRepositoryMock); 
                        services.AddScoped(_ => productRepositoryMock);
                        services.AddScoped(_ => productSupplierRepositoryMock);
                        services.AddScoped(_ => supplierRepositoryMock);
                    });
                });

            HttpClient = _factory.CreateClient();
        }

        protected TClient CreateApiClient<TClient>() where TClient : class
        {
            return (TClient)Activator.CreateInstance(typeof(TClient), HttpClient)!;
        }

        public void Dispose()
        {
            HttpClient.Dispose();
            _factory.Dispose();
        }
    }
}
