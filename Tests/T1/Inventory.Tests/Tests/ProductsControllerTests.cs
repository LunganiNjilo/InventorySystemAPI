using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Inventory.Tests.Infrastructure;
using NSubstitute;
using System.Linq.Expressions;
using System.Net;

namespace Inventory.Tests.Tests
{
    internal class ProductsControllerTests : TestBase
    {
        private InventoryApiClient _client;

        [SetUp]
        public void Setup()
        {
            _client = CreateApiClient<InventoryApiClient>();
            productRepositoryMock.ClearReceivedCalls();
        }

        [Test]
        public async Task GetProducts_ReturnsPagedProducts_WhenDataExists()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var products = new List<Product>
    {
        new Product
        {
            Id = Guid.NewGuid(),
            ProductName = "Coca-Cola",
            ProductDescription = "Drink",
            SellPrice = 10,
            CostPrice = 7,
            FkProductCategory = categoryId,
            ProductCategory = new ProductCategory
            {
                Id = categoryId,
                ProductCategoryName = "Drinks"
            }
        }
    };

            productRepositoryMock
                .SearchSortAndPaginationAsync(
                    Arg.Any<string?>(),
                    Arg.Any<string?>(),
                    Arg.Any<string?>(),
                    Arg.Any<bool>(),
                    Arg.Any<int>(),
                    Arg.Any<int>()
                )
                .Returns((
                    (ICollection<Product>)products,
                    1,
                    1,
                    "Page 1 from 1 pages",
                    false,
                    false
                ));

            // Act
            var response = await _client.GetAsync<dynamic>("/api/Products");

            // Assert (✅ CORRECT CONTRACT)
            Assert.That(response.result, Is.Not.Null);
            Assert.That(response.result.Count, Is.EqualTo(1));
            Assert.That(
                response.result[0].productName.ToString(),
                Is.EqualTo("Coca-Cola")
            );

            Assert.That((int)response.totalRecordCount, Is.EqualTo(1));
            Assert.That((int)response.totalPages, Is.EqualTo(1));

            await productRepositoryMock.Received(1)
                .SearchSortAndPaginationAsync(
                    Arg.Any<string?>(),
                    Arg.Any<string?>(),
                    Arg.Any<string?>(),
                    Arg.Any<bool>(),
                    Arg.Any<int>(),
                    Arg.Any<int>()
                );
        }



        [Test]
        public async Task GetProducts_ReturnsNotFound_WhenNoData()
        {
            // Arrange
            productRepositoryMock
                .SearchSortAndPaginationAsync(
                    Arg.Any<string?>(),
                    Arg.Any<string?>(),
                    Arg.Any<string?>(),
                    Arg.Any<bool>(),
                    Arg.Any<int>(),
                    Arg.Any<int>())
                .Returns((
                    new List<Product>(),
                    0,
                    0,
                    "",
                    false,
                    false
                ));

            // Act
            var response = await _client.GetRawAsync("/api/Products");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Create_Should_AddInventory()
        {
            // Arrange
            Domain.Entities.Inventory? savedEntity = null;

            inventoryRepositoryMock
                .AddAsync(Arg.Do<Domain.Entities.Inventory>(inv =>
                {
                    inv.Id = Guid.NewGuid();   // simulate EF assigning ID
                    savedEntity = inv;
                }))
                .Returns(ci => ci.Arg<Domain.Entities.Inventory>());

            inventoryRepositoryMock
                .GetByIdAsync(Arg.Any<Guid>())
                .Returns(ci => savedEntity);

            var dto = new InventoryCreateDto
            {
                FkProductId = Guid.NewGuid(),
                QuantityInStock = 5,
                MinStockLevel = 1,
                MaxStockLevel = 10
            };

            // Act
            var response = await _client.PostAsync<InventoryResponseDto>(
                "/api/Inventory", dto);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.QuantityInStock, Is.EqualTo(5));

            await inventoryRepositoryMock.Received(1)
                .AddAsync(Arg.Any<Domain.Entities.Inventory>());
        }


        [Test]
        public async Task CreateProduct_ReturnsCreatedProduct()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var request = new CreateProductRequest
            {
                ProductName = "Fanta",
                ProductDescription = "Cool drink",
                SellPrice = 10,
                CostPrice = 5,
                FkProductCategory = categoryId
            };

            var product = new Product
            {
                Id = Guid.NewGuid(),
                ProductName = "Fanta",
                ProductDescription = "Cool drink",
                SellPrice = 10,
                CostPrice = 5,
                ProductCategory = new ProductCategory
                {
                    Id = categoryId,
                    ProductCategoryName = "Beverage"
                }
            };

            productRepositoryMock
                .CreateAsync(Arg.Any<Product>())
                .Returns(product);

            productRepositoryMock
                .GetEntityWithSpecAsync(Arg.Any<ISpecification<Product>>())
                .Returns(product);

            // Act
            var response =
                await _client.PostAsync<ApiResponse<ProductDto>>("/api/Products", request);

            // Assert
            Assert.That(response.Success, Is.True);
            Assert.That(response.Result.ProductName, Is.EqualTo("Fanta"));
            Assert.That(response.Result.ProductCategoryName, Is.EqualTo("Beverage"));
        }


        [Test]
        public async Task UpdateProduct_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();

            productRepositoryMock
                .GetByIdAsync(id)
                .Returns(new Product { Id = id });

            var request = new UpdateProductRequest
            {
                ProductName = "Updated"
            };

            // Act
            var response = await _client.PutRawAsync(
                $"/api/Products/{id}", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            await productRepositoryMock.Received()
                .UpdateAsync(Arg.Any<Product>());
        }

        [Test]
        public async Task DeleteProduct_ReturnsNoContent_WhenSuccess()
        {
            // Arrange
            var id = Guid.NewGuid();

            productRepositoryMock
                .GetByIdAsync(id)
                .Returns(new Product { Id = id });

            // Act
            var response = await _client.DeleteRawAsync(
                $"/api/Products/{id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            await productRepositoryMock.Received()
                .DeleteAsync(Arg.Any<Product>());
        }
    }
}
