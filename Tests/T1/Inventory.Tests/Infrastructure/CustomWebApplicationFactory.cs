using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Tests.Infrastructure
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly Action<IServiceCollection> _configureTestServices;

        public CustomWebApplicationFactory(Action<IServiceCollection> configureTestServices)
        {
            _configureTestServices = configureTestServices;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var mapperDescriptors = services
                    .Where(d => d.ServiceType == typeof(IMapper))
                    .ToList();

                foreach (var descriptor in mapperDescriptors)
                    services.Remove(descriptor);

                services.AddAutoMapper(cfg =>
                {
                    cfg.AddProfile<Application.Mappers.ProductSupplierProfile>();
                    cfg.AddProfile<Application.Mappers.ProductProfile>();
                    cfg.AddProfile<Application.Mappers.InventoryProfile>();
                    cfg.AddProfile<Application.Mappers.ProductCategoryProfile>();
                });

                _configureTestServices?.Invoke(services);
            });
        }

    }
}
