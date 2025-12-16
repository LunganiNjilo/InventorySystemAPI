using Application.DTOs;
using Domain.Entities;
using Inventory.Tests.Infrastructure;
using NSubstitute;
using System.Net;

namespace ProductCategories.Tests.Tests
{
    internal class ProductCategoriesControllerTests : TestBase
    {
        private InventoryApiClient _client;

        [SetUp]
        public void Setup()
        {
            _client = CreateApiClient<InventoryApiClient>();
            productCategoryRepositoryMock.ClearReceivedCalls();
        }
        [Test]
        public async Task GetAll_ReturnsCategories()
        {
            // Arrange
            var categories = new List<ProductCategory> 
            {
                new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    ProductCategoryName = "Drinks"
                }
            };

            productCategoryRepositoryMock
                .GetAllAsync()
                .Returns(categories);

            // Act
            var response = await _client.GetAsync<ApiListResponse<ProductCategoryReadDto>>("/api/ProductCategories");

            // Assert
            Assert.That(response.Result.Count, Is.EqualTo(1));
            Assert.That(response.Result[0].ProductCategoryName, Is.EqualTo("Drinks"));
        }

        [Test]
        public async Task Get_ReturnsCategory_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            var category = new ProductCategory
            {
                Id = id,
                ProductCategoryName = "Drinks"
            };

            productCategoryRepositoryMock
                .GetByIdAsync(id)
                .Returns(category);

            // Act
            var response = await _client.GetAsync<
                ApiResponse<ProductCategoryReadDto>
            >($"/api/ProductCategories/{id}");

            // Assert
            Assert.That(response.Result.Id, Is.EqualTo(id));
            Assert.That(response.Result.ProductCategoryName, Is.EqualTo("Drinks"));

            await productCategoryRepositoryMock.Received(1).GetByIdAsync(id);
        }

        [Test]
        public async Task Create_Should_CreateCategory()
        {
            // Arrange
            var dto = new ProductCategoryCreateDto
            {
                ProductCategoryName = "Beverages"
            };

            var created = new ProductCategory
            {
                Id = Guid.NewGuid(),
                ProductCategoryName = "Beverages"
            };

            productCategoryRepositoryMock
                .CreateAsync(Arg.Any<ProductCategory>())
                .Returns(created);

            // Act
            var response = await _client.PostAsync<ProductCategoryReadDto>(
                "/api/ProductCategories",
                dto
            );

            // Assert
            Assert.That(response.Id, Is.EqualTo(created.Id));
            Assert.That(response.ProductCategoryName, Is.EqualTo("Beverages"));

            await productCategoryRepositoryMock.Received(1)
                .CreateAsync(Arg.Any<ProductCategory>());
        }

        [Test]
        public async Task Update_Should_UpdateCategory()
        {
            // Arrange
            var id = Guid.NewGuid();

            var existing = new ProductCategory
            {
                Id = id,
                ProductCategoryName = "Old"
            };

            productCategoryRepositoryMock
                .GetByIdAsync(id)
                .Returns(existing);

            productCategoryRepositoryMock
                .UpdateAsync(existing)
                .Returns(Task.CompletedTask);

            var dto = new ProductCategoryCreateDto
            {
                ProductCategoryName = "New"
            };

            // Act
            var response = await _client.PutAsync<ApiResponse<ProductCategoryReadDto>>($"/api/ProductCategories/{id}", dto);

            // Assert
            Assert.That(response.Result.ProductCategoryName, Is.EqualTo("New"));

            await productCategoryRepositoryMock.Received(1)
                .UpdateAsync(Arg.Is<ProductCategory>(
                    x => x.ProductCategoryName == "New"
                ));
        }

        [Test]
        public async Task Delete_Should_ReturnOk_And_DeleteCategory()
        {
            // Arrange
            var id = Guid.NewGuid();

            var category = new ProductCategory
            {
                Id = id,
                ProductCategoryName = "ToDelete"
            };

            productCategoryRepositoryMock
                .GetByIdAsync(id)
                .Returns(category);

            productCategoryRepositoryMock
                .DeleteAsync(category)
                .Returns(Task.CompletedTask);

            // Act
            var response = await _client.DeleteRawAsync(
                $"/api/ProductCategories/{id}"
            );

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            await productCategoryRepositoryMock.Received(1)
                .DeleteAsync(category);
        }

    }
}
