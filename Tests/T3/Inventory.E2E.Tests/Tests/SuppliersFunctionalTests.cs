using System.Net;
using System.Net.Http.Json;
using Inventory.E2E.Tests.Models;
using NUnit.Framework;

namespace Inventory.Api.FunctionalTests.Tests
{
    internal class SuppliersFunctionalTests : FunctionalTestBase
    {
        [Test]
        public async Task Suppliers_CRUD_HappyPath_Works_EndToEnd()
        {
            // =========================
            // 1. CREATE SUPPLIER
            // =========================
            var createRequest = new
            {
                supplierName = "Functional Supplier",
                supplierAddress = "123 Test Street",
                contactFirstName = "John",
                contactLastName = "Doe",
                contactEmail = "john.supplier@test.com"
            };

            var createResponse = await Client.PostAsJsonAsync(
                "/api/Suppliers",
                createRequest);

            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var createdSupplier =
                await createResponse.Content.ReadFromJsonAsync<SupplierReadDto>();

            var supplierId = Guid.Parse(createdSupplier.Id.ToString());

            Assert.That(
                createdSupplier.SupplierName.ToString(),
                Is.EqualTo("Functional Supplier"));

            // =========================
            // 2. GET SUPPLIER BY ID
            // =========================
            var getResponse = await Client.GetAsync(
                $"/api/Suppliers/{supplierId}");

            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var fetched =
                await getResponse.Content.ReadFromJsonAsync<SupplierReadDto>();

            Assert.That(
                fetched.Id.ToString(),
                Is.EqualTo(supplierId.ToString()));

            // =========================
            // 3. GET SUPPLIERS (PAGED)
            // =========================
            var listResponse = await Client.GetAsync("/api/Suppliers");

            Assert.That(listResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var paged =
                await listResponse.Content.ReadFromJsonAsync<PagedSupplierResponseDto>();

            Assert.That(paged.Result.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That((int)paged.TotalRecordCount, Is.GreaterThanOrEqualTo(1));

            // =========================
            // 4. UPDATE SUPPLIER
            // =========================
            var updateRequest = new
            {
                supplierName = "Updated Functional Supplier",
                supplierAddress = "456 Updated Street",
                contactFirstName = "Jane",
                contactLastName = "Doe",
                contactEmail = "jane.supplier@test.com"
            };

            var updateResponse = await Client.PutAsJsonAsync(
                $"/api/Suppliers/{supplierId}",
                updateRequest);

            Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            // =========================
            // 5. VERIFY UPDATE
            // =========================
            var verifyResponse = await Client.GetAsync(
                $"/api/Suppliers/{supplierId}");

            var updated =
                await verifyResponse.Content.ReadFromJsonAsync<SupplierReadDto>();

            Assert.That(
                updated.SupplierName.ToString(),
                Is.EqualTo("Updated Functional Supplier"));

            // =========================
            // 6. DELETE SUPPLIER
            // =========================
            var deleteResponse = await Client.DeleteAsync(
                $"/api/Suppliers/{supplierId}");

            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            // =========================
            // 7. VERIFY DELETE
            // =========================
            var deletedGet = await Client.GetAsync(
                $"/api/Suppliers/{supplierId}");

            Assert.That(deletedGet.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetSupplier_Returns_NotFound_For_Unknown_Id()
        {
            var response = await Client.GetAsync(
                $"/api/Suppliers/{Guid.NewGuid()}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
