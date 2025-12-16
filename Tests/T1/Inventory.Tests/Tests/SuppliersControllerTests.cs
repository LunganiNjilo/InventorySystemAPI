using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Inventory.Tests.Infrastructure;
using NSubstitute;
using System.Net;

namespace Inventory.Tests.Tests
{
    internal class SuppliersControllerTests : TestBase
    {
        private InventoryApiClient _client;

        [SetUp]
        public void Setup()
        {
            _client = CreateApiClient<InventoryApiClient>();
            supplierRepositoryMock.ClearReceivedCalls();
        }

        [Test]
        public async Task GetSuppliers_ReturnsPagedSuppliers_WhenDataExists()
        {
            // Arrange
            var suppliers = new List<Supplier>
            {
                new Supplier
                {
                    Id = Guid.NewGuid(),
                    SupplierName = "ABC Supplies",
                    SupplierAddress = "123 Street",
                    ContactFirstName = "John",
                    ContactLastName = "Doe",
                    ContactEmail = "john@abc.com"
                }
            };

            supplierRepositoryMock
                .SearchSortAndPaginationAsync(
                    Arg.Any<string?>(),
                    Arg.Any<string?>(),
                    Arg.Any<string?>(),
                    Arg.Any<bool>(),
                    Arg.Any<int>(),
                    Arg.Any<int>())
                .Returns((
                    (ICollection<Supplier>)suppliers,
                    1,
                    1,
                    "Page 1 from 1 pages",
                    false,
                    false
                ));

            // Act
            var response = await _client.GetAsync<dynamic>("/api/Suppliers");

            // Assert (✅ SAME CONTRACT AS PRODUCTS)
            Assert.That(response.result, Is.Not.Null);
            Assert.That(response.result.Count, Is.EqualTo(1));
            Assert.That(
                response.result[0].supplierName.ToString(),
                Is.EqualTo("ABC Supplies")
            );

            Assert.That((int)response.totalRecordCount, Is.EqualTo(1));
            Assert.That((int)response.totalPages, Is.EqualTo(1));

            await supplierRepositoryMock.Received(1)
                .SearchSortAndPaginationAsync(
                    Arg.Any<string?>(),
                    Arg.Any<string?>(),
                    Arg.Any<string?>(),
                    Arg.Any<bool>(),
                    Arg.Any<int>(),
                    Arg.Any<int>());
        }

        [Test]
        public async Task GetSuppliers_ReturnsNotFound_WhenNoData()
        {
            // Arrange
            supplierRepositoryMock
                .SearchSortAndPaginationAsync(
                    Arg.Any<string?>(),
                    Arg.Any<string?>(),
                    Arg.Any<string?>(),
                    Arg.Any<bool>(),
                    Arg.Any<int>(),
                    Arg.Any<int>())
                .Returns((
                    new List<Supplier>(),
                    0,
                    0,
                    "",
                    false,
                    false
                ));

            // Act
            var response = await _client.GetRawAsync("/api/Suppliers");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetSupplier_ReturnsSupplier_WhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();

            supplierRepositoryMock
                .GetByIdAsync(id)
                .Returns(new Supplier
                {
                    Id = id,
                    SupplierName = "ABC Supplies"
                });

            // Act
            var response = await _client.GetAsync<dynamic>(
                $"/api/Suppliers/{id}");

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(
                response.supplierName.ToString(),
                Is.EqualTo("ABC Supplies"));
        }

        [Test]
        public async Task GetSupplier_ReturnsNotFound_WhenMissing()
        {
            // Arrange
            supplierRepositoryMock
                .GetByIdAsync(Arg.Any<Guid>())
                .Returns((Supplier?)null);

            // Act
            var response = await _client.GetRawAsync(
                $"/api/Suppliers/{Guid.NewGuid()}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task CreateSupplier_ReturnsCreatedSupplier()
        {
            // Arrange
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                SupplierName = "New Supplier"
            };

            supplierRepositoryMock
                .CreateAsync(Arg.Any<Supplier>())
                .Returns(supplier);

            supplierRepositoryMock
                .GetByIdAsync(Arg.Any<Guid>())
                .Returns(supplier);

            var request = new SupplierCreateDto
            {
                SupplierName = "New Supplier",
                SupplierAddress = "Address",
                ContactFirstName = "Jane",
                ContactLastName = "Doe",
                ContactEmail = "jane@test.com"
            };

            // Act
            var response = await _client.PostAsync<dynamic>(
                "/api/Suppliers", request);

            // Assert
            Assert.That(
                response.supplierName.ToString(),
                Is.EqualTo("New Supplier"));
        }

        [Test]
        public async Task UpdateSupplier_ReturnsNoContent_WhenSuccess()
        {
            // Arrange
            var id = Guid.NewGuid();

            supplierRepositoryMock
                .GetByIdAsync(id)
                .Returns(new Supplier { Id = id });

            var request = new SupplierUpdateDto
            {
                SupplierName = "Updated Supplier"
            };

            // Act
            var response = await _client.PutRawAsync(
                $"/api/Suppliers/{id}", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            await supplierRepositoryMock.Received()
                .UpdateAsync(Arg.Any<Supplier>());
        }

        [Test]
        public async Task DeleteSupplier_ReturnsNoContent_WhenSuccess()
        {
            // Arrange
            var id = Guid.NewGuid();

            supplierRepositoryMock
                .GetByIdAsync(id)
                .Returns(new Supplier { Id = id });

            // Act
            var response = await _client.DeleteRawAsync(
                $"/api/Suppliers/{id}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            await supplierRepositoryMock.Received()
                .DeleteAsync(Arg.Any<Supplier>());
        }
    }
}
