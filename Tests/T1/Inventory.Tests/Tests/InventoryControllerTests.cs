using Application.DTOs;
using Domain.Entities;
using Inventory.Tests.Infrastructure;
using NSubstitute;
using System.Net;

namespace Inventory.Tests.Tests
{
    internal class InventoryControllerTests : TestBase
    {
        private InventoryApiClient _client;

        [SetUp]
        public void Setup()
        {
            _client = CreateApiClient<InventoryApiClient>();
            inventoryRepositoryMock.ClearReceivedCalls();
        }

        [Test]
        public async Task Search_ReturnsFilteredInventory()
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                ProductName = "Coca-Cola"
            };

            var inventory = new Domain.Entities.Inventory
            {
                Id = Guid.NewGuid(),
                Product = product,
                QuantityInStock = 10
            };

            inventoryRepositoryMock.GetAllAsync()
                .Returns(new List<Domain.Entities.Inventory> { inventory });

            var result = await _client.GetAsync<List<InventoryResponseDto>>(
                "/api/Inventory?search=coca&sortBy=product");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].ProductName, Is.EqualTo("Coca-Cola"));
        }

        [Test]
        public async Task Get_ReturnsNotFound_WhenMissing()
        {
            var id = Guid.NewGuid();
            inventoryRepositoryMock.GetByIdAsync(id).Returns((Domain.Entities.Inventory?)null);

            var response = await _client.GetRawAsync($"/api/Inventory/{id}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Create_Should_AddInventory()
        {
            // Arrange
            var generatedId = Guid.NewGuid();

            inventoryRepositoryMock
                .AddAsync(Arg.Any<Domain.Entities.Inventory>())
                .Returns(callInfo =>
                {
                    var entity = callInfo.Arg<Domain.Entities.Inventory>();
                    entity.Id = generatedId; // ⬅ simulate EF
                    return entity;
                });

            inventoryRepositoryMock
                .GetByIdAsync(generatedId)
                .Returns(new Domain.Entities.Inventory
                {
                    Id = generatedId,
                    QuantityInStock = 5,
                    Product = new Product { ProductName = "Coke" }
                });

            var dto = new InventoryCreateDto
            {
                FkProductId = Guid.NewGuid(),
                QuantityInStock = 5,
                MinStockLevel = 1,
                MaxStockLevel = 10
            };

            // Act
            var result = await _client.PostAsync<InventoryResponseDto>(
                "/api/Inventory", dto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(generatedId));
            Assert.That(result.ProductName, Is.EqualTo("Coke"));

            await inventoryRepositoryMock.Received(1)
                .AddAsync(Arg.Any<Domain.Entities.Inventory>());
        }


        [Test]
        public async Task Update_Should_ReturnNoContent()
        {
            var id = Guid.NewGuid();

            inventoryRepositoryMock.GetByIdAsync(id)
                .Returns(new Domain.Entities.Inventory { Id = id });

            var response = await _client.PutRawAsync(
                $"/api/Inventory/{id}",
                new InventoryUpdateDto
                {
                    QuantityInStock = 20,
                    MinStockLevel = 5,
                    MaxStockLevel = 50
                });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            await inventoryRepositoryMock.Received().UpdateAsync(Arg.Any<Domain.Entities.Inventory>());
        }

        [Test]
        public async Task Delete_Should_ReturnNoContent_WhenExists()
        {
            var id = Guid.NewGuid();

            inventoryRepositoryMock.GetByIdAsync(id)
                .Returns(new Domain.Entities.Inventory { Id = id });

            var response = await _client.DeleteRawAsync($"/api/Inventory/{id}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            await inventoryRepositoryMock.Received().DeleteAsync(Arg.Any<Domain.Entities.Inventory>());
        }
    }
}
