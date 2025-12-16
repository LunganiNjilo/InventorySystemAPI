using Inventory.E2E.Tests.Models;
using System.Net;
using System.Net.Http.Json;

namespace Inventory.Api.FunctionalTests.Tests
{
    internal class InventoryFunctionalTests : FunctionalTestBase
    {
        [Test]
        public async Task Inventory_CRUD_HappyPath_Works_EndToEnd()
        {
            // =========================
            // CREATE CATEGORY
            // =========================
            var createCategoryRequest = new
            {
                productCategoryName = "Functional Test Category"
            };

            var createCategoryResponse = await Client.PostAsJsonAsync(
                "/api/ProductCategories",
                createCategoryRequest);

            Assert.That(createCategoryResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var category =
                await createCategoryResponse.Content.ReadFromJsonAsync<ProductCategoryReadDto>();

            var categoryId = category!.Id;

            // =========================
            // CREATE PRODUCT
            // =========================
            var createProductRequest = new
            {
                productName = "Inventory Functional Product",
                productDescription = "Used for Inventory E2E test",
                sellPrice = 100,
                costPrice = 70,
                fkProductCategory = categoryId
            };

            var productResponse = await Client.PostAsJsonAsync(
                "/api/Products",
                createProductRequest);

            Assert.That(productResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var product =
                await productResponse.Content.ReadFromJsonAsync<CreateProductsDTO>();

            var productId = product!.result.Id;

            // =========================
            // CREATE INVENTORY
            // =========================
            var createInventoryRequest = new
            {
                fkProductId = productId,
                quantityInStock = 10,
                minStockLevel = 2,
                maxStockLevel = 50
            };

            var createInventoryResponse = await Client.PostAsJsonAsync(
                "/api/Inventory",
                createInventoryRequest);

            Assert.That(createInventoryResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var inventory =
                await createInventoryResponse.Content.ReadFromJsonAsync<InventoryResponseDto>();

            Assert.That((int)inventory!.QuantityInStock, Is.EqualTo(10));

            var inventoryId = inventory.Id;

            // =========================
            // GET INVENTORY
            // =========================
            var getResponse = await Client.GetAsync($"/api/Inventory/{inventoryId}");
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // =========================
            // UPDATE INVENTORY
            // =========================
            var updateInventoryRequest = new
            {
                quantityInStock = 25,
                minStockLevel = 5,
                maxStockLevel = 100
            };

            var updateResponse = await Client.PutAsJsonAsync(
                $"/api/Inventory/{inventoryId}",
                updateInventoryRequest);

            Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            // =========================
            // VERIFY UPDATE
            // =========================
            var verifyResponse = await Client.GetAsync($"/api/Inventory/{inventoryId}");
            var updated =
                await verifyResponse.Content.ReadFromJsonAsync<InventoryResponseDto>();

            Assert.That((int)updated!.QuantityInStock, Is.EqualTo(25));
            Assert.That((int)updated.MinStockLevel, Is.EqualTo(5));
            Assert.That((int)updated.MaxStockLevel, Is.EqualTo(100));

            // =========================
            // DELETE INVENTORY
            // =========================
            var deleteInventoryResponse =
                await Client.DeleteAsync($"/api/Inventory/{inventoryId}");

            Assert.That(deleteInventoryResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            // =========================
            // CLEANUP PRODUCT
            // =========================
            var deleteProductResponse =
                await Client.DeleteAsync($"/api/Products/{productId}");

            Assert.That(deleteProductResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            // =========================
            // CLEANUP CATEGORY 
            // =========================
            var deleteCategoryResponse =
                await Client.DeleteAsync($"/api/ProductCategories/{categoryId}");

            Assert.That(deleteCategoryResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
