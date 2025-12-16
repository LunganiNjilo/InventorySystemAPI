using System.Net;
using System.Net.Http.Json;
using Inventory.E2E.Tests.Models;
using NUnit.Framework;

namespace Inventory.Api.FunctionalTests.Tests
{
    internal class ProductsFunctionalTests : FunctionalTestBase
    {
        [Test]
        public async Task Products_CRUD_HappyPath_Works_EndToEnd()
        {
            // =========================
            // PRECONDITION: CREATE CATEGORY
            // (Product requires category)
            // =========================
            var createCategoryRequest = new
            {
                productCategoryName = "Products Functional Category"
            };

            var categoryResponse = await Client.PostAsJsonAsync(
                "/api/ProductCategories",
                createCategoryRequest);

            Assert.That(categoryResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var category = await categoryResponse.Content.ReadFromJsonAsync<ProductCategoryReadDto>();
            var categoryId = Guid.Parse(category!.Id.ToString());

            // =========================
            // CREATE PRODUCT
            // =========================
            var createProductRequest = new
            {
                productName = "Functional Product",
                productDescription = "E2E product test",
                sellPrice = 100,
                costPrice = 70,
                fkProductCategory = categoryId
            };

            var createResponse = await Client.PostAsJsonAsync(
                "/api/Products",
                createProductRequest);

            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var createdBody = await createResponse.Content.ReadFromJsonAsync<CreateProductsDTO>();
            Assert.That(createdBody!.success, Is.True);

            var productId = Guid.Parse(createdBody.result.Id.ToString());

            // =========================
            // GET PRODUCT BY ID
            // =========================
            var getResponse = await Client.GetAsync(
                $"/api/Products/{productId}");

            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var getBody = await getResponse.Content.ReadFromJsonAsync<GetProductsDTO>();
            Assert.That(getBody!.success, Is.True);
            Assert.That(
                getBody.result.ProductName!.ToString(),
                Is.EqualTo("Functional Product"));

            // =========================
            // GET PRODUCTS (LIST)
            // =========================
            var listResponse = await Client.GetAsync("/api/Products");

            Assert.That(listResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var listBody = await listResponse.Content.ReadFromJsonAsync<PagedProductResponseDto>();
            Assert.That(listBody!.Success, Is.True);
            Assert.That(listBody.Result.Count, Is.GreaterThanOrEqualTo(1));

            // =========================
            // UPDATE PRODUCT
            // =========================
            var updateRequest = new
            {
                productName = "Updated Functional Product",
                productDescription = "Updated",
                sellPrice = 120,
                costPrice = 80,
                fkProductCategory = categoryId
            };

            var updateResponse = await Client.PutAsJsonAsync(
                $"/api/Products/{productId}",
                updateRequest);

            Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            // =========================
            // DELETE PRODUCT
            // =========================
            var deleteResponse = await Client.DeleteAsync(
                $"/api/Products/{productId}");

            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            // =========================
            // CLEANUP CATEGORY
            // =========================
            var deleteCategoryResponse = await Client.DeleteAsync(
                $"/api/ProductCategories/{categoryId}");

            Assert.That(deleteCategoryResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
