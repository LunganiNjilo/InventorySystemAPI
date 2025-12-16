using System.Net;
using System.Net.Http.Json;
using Inventory.E2E.Tests.Models;
using NUnit.Framework;

namespace Inventory.Api.FunctionalTests.Tests
{
    internal class ProductSuppliersFunctionalTests : FunctionalTestBase
    {
        [Test]
        public async Task ProductSuppliers_CRUD_HappyPath_Works_EndToEnd()
        {
            // =========================
            // 1. CREATE CATEGORY
            // =========================
            var categoryResponse = await Client.PostAsJsonAsync(
                "/api/ProductCategories",
                new { productCategoryName = "PS Functional Category" });

            Assert.That(categoryResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var category = await categoryResponse.Content.ReadFromJsonAsync<ProductSupplierReadDto>();
            var categoryId = Guid.Parse(category!.Id.ToString());

            // =========================
            // 2. CREATE PRODUCT
            // =========================
            var productResponse = await Client.PostAsJsonAsync(
                "/api/Products",
                new
                {
                    productName = "PS Functional Product",
                    productDescription = "Used for ProductSupplier test",
                    sellPrice = 50,
                    costPrice = 30,
                    fkProductCategory = categoryId
                });

            Assert.That(productResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var productBody = await productResponse.Content.ReadFromJsonAsync<CreateProductsDTO>();
            var productId = Guid.Parse(productBody!.result.Id.ToString());

            // =========================
            // 3. CREATE SUPPLIER
            // =========================
            var supplierResponse = await Client.PostAsJsonAsync(
                "/api/Suppliers",
                new
                {
                    supplierName = "PS Functional Supplier",
                    supplierAddress = "123 Street",
                    contactFirstName = "John",
                    contactLastName = "Doe",
                    contactEmail = "john@ps-test.com"
                });

            Assert.That(supplierResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var supplier = await supplierResponse.Content.ReadFromJsonAsync<SupplierReadDto>();
            var supplierId = Guid.Parse(supplier!.Id.ToString());

            // =========================
            // 4. CREATE PRODUCT SUPPLIER
            // =========================
            var psResponse = await Client.PostAsJsonAsync(
                "/api/ProductSuppliers",
                new
                {
                    fkProductId = productId,
                    fkSupplierId = supplierId,
                    costPrice = 25
                });

            Assert.That(psResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var productSupplier = await psResponse.Content.ReadFromJsonAsync<ProductSupplierReadDto>();
            var productSupplierId = Guid.Parse(productSupplier!.Id.ToString());

            // =========================
            // 5. GET PRODUCT SUPPLIER
            // =========================
            var getResponse = await Client.GetAsync(
                $"/api/ProductSuppliers/{productSupplierId}");

            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // =========================
            // 6. UPDATE PRODUCT SUPPLIER
            // =========================
            var updateResponse = await Client.PutAsJsonAsync(
                $"/api/ProductSuppliers/{productSupplierId}",
                new
                {
                    fkProductId = productId,
                    fkSupplierId = supplierId,
                    costPrice = 28
                });

            Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // =========================
            // 7. DELETE PRODUCT SUPPLIER
            // =========================
            var deletePsResponse = await Client.DeleteAsync(
                $"/api/ProductSuppliers/{productSupplierId}");

            Assert.That(deletePsResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            // =========================
            // 8. CLEANUP (reverse order)
            // =========================
            await Client.DeleteAsync($"/api/Products/{productId}");
            await Client.DeleteAsync($"/api/Suppliers/{supplierId}");
            await Client.DeleteAsync($"/api/ProductCategories/{categoryId}");
        }
    }
}
