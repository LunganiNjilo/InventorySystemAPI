using Application.DTOs;
using Domain.Entities;
using Inventory.Tests.Infrastructure;
using NSubstitute;
using System.Net;

namespace Inventory.Tests.Tests
{
    internal class ProductSuppliersControllerTests : TestBase
    {
        private InventoryApiClient _client;

        [SetUp]
        public void Setup()
        {
            _client = CreateApiClient<InventoryApiClient>();
            productSupplierRepositoryMock.ClearReceivedCalls();
        }

        [Test]
        public async Task GetAll_ReturnsProductSuppliers()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var supplierId = Guid.NewGuid();

            var items = new List<ProductSupplier>
            {
                new ProductSupplier
                {
                    Id = Guid.NewGuid(),
                    FkProductId = productId,
                    FkSupplierId = supplierId,
                    Product = new Product
                    {
                        Id = productId,
                        ProductName = "Coke"
                    },
                    Supplier = new Supplier
                    {
                        Id = supplierId,
                        SupplierName = "Coca-Cola SA"
                    }
                }
            };

            productSupplierRepositoryMock
                .GetAllAsync()
                .Returns(items);

            // Act
            var response = await _client.GetAsync<List<ProductSupplierReadDto>>(
                "/api/ProductSuppliers");

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Count, Is.EqualTo(1));
            Assert.That(response[0].ProductName, Is.EqualTo("Coke"));
            Assert.That(response[0].SupplierName, Is.EqualTo("Coca-Cola SA"));

            await productSupplierRepositoryMock.Received(1).GetAllAsync();
        }

        [Test]
        public async Task Get_ReturnsProductSupplier_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            productSupplierRepositoryMock
                .GetByIdAsync(id)
                .Returns(new ProductSupplier
                {
                    Id = id,
                    Product = new Product { ProductName = "Sprite" },
                    Supplier = new Supplier { SupplierName = "Coca-Cola SA" }
                });

            // Act
            var response = await _client.GetAsync<ProductSupplierReadDto>(
                $"/api/ProductSuppliers/{id}");

            // Assert
            Assert.That(response.Id, Is.EqualTo(id));
            Assert.That(response.ProductName, Is.EqualTo("Sprite"));

            await productSupplierRepositoryMock.Received(1)
                .GetByIdAsync(id);
        }

        [Test]
        public async Task Create_ReturnsCreatedProductSupplier()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var supplierId = Guid.NewGuid();
            var psId = Guid.NewGuid();

            var createdEntity = new ProductSupplier
            {
                Id = psId,
                FkProductId = productId,
                FkSupplierId = supplierId,
                Product = new Product
                {
                    Id = productId,
                    ProductName = "Coca-Cola"
                },
                Supplier = new Supplier
                {
                    Id = supplierId,
                    SupplierName = "Coca-Cola SA"
                }
            };

            productSupplierRepositoryMock
                .CreateAsync(Arg.Any<ProductSupplier>())
                .Returns(createdEntity);

            var dto = new ProductSupplierCreateDto
            {
                FkProductId = productId,
                FkSupplierId = supplierId
            };

            // Act
            var response = await _client.PostAsync<ProductSupplierReadDto>(
                "/api/ProductSuppliers",
                dto);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ProductName, Is.EqualTo("Coca-Cola"));
            Assert.That(response.SupplierName, Is.EqualTo("Coca-Cola SA"));

            await productSupplierRepositoryMock.Received(1)
                .CreateAsync(Arg.Any<ProductSupplier>());
        }


        [Test]
        public async Task Delete_ReturnsNoContent_WhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();

            productSupplierRepositoryMock
                .GetByIdAsync(id)
                .Returns(new ProductSupplier { Id = id });

            // Act
            var response = await _client.DeleteRawAsync(
                $"/api/ProductSuppliers/{id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            await productSupplierRepositoryMock.Received(1)
                .DeleteAsync(Arg.Any<ProductSupplier>());
        }
    }
}
