using Inventory.E2E.Tests.Models;
using System.Net;
using System.Net.Http.Json;

namespace Inventory.E2E.Tests.Tests
{
    internal class ProductCategoriesFunctionalTests : FunctionalTestBase
    {
        [Test]
        public async Task ProductCategories_CRUD_HappyPath_Works_EndToEnd()
        {
            // =========================
            // CREATE CATEGORY
            // =========================
            var createRequest = new
            {
                productCategoryName = "Functional Test Category"
            };

            var createResponse = await Client.PostAsJsonAsync(
                "/api/ProductCategories",
                createRequest);

            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            // POST returns entity directly
            var created = await createResponse.Content.ReadFromJsonAsync<ProductCategoryReadDto>();
            Assert.That(created, Is.Not.Null);

            var categoryId = Guid.Parse(created.Id.ToString());
            Assert.That(
                created!.ProductCategoryName.ToString(),
                Is.EqualTo("Functional Test Category"));

            // =========================
            // GET BY ID
            // =========================
            var getResponse = await Client.GetAsync(
                $"/api/ProductCategories/{categoryId}");

            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var getBody = await getResponse.Content.ReadFromJsonAsync<GetProductCategoryDTO>();
            Assert.That(getBody!.result!.Id.ToString(), Is.EqualTo(categoryId.ToString()));

            // =========================
            // GET ALL
            // =========================
            var getAllResponse = await Client.GetAsync("/api/ProductCategories");
            Assert.That(getAllResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var list = await getAllResponse.Content.ReadFromJsonAsync<GetAllProductCategoryDTO>();
            Assert.That(list!.result!.Count, Is.GreaterThanOrEqualTo(1));

            // =========================
            // UPDATE
            // =========================
            var updateRequest = new
            {
                productCategoryName = "Updated Functional Category"
            };

            var updateResponse = await Client.PutAsJsonAsync(
                $"/api/ProductCategories/{categoryId}",
                updateRequest);

            Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var updated = await updateResponse.Content.ReadFromJsonAsync<UpdateCategoryDTO>();
            Assert.That(
                updated!.result.ProductCategoryName.ToString(),
                Is.EqualTo("Updated Functional Category"));

            // =========================
            // DELETE
            // =========================
            var deleteResponse = await Client.DeleteAsync(
                $"/api/ProductCategories/{categoryId}");

            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var deleteBody = await deleteResponse.Content.ReadFromJsonAsync<DeleteCategoryDTO>();
            Assert.That(
                deleteBody!.message!.ToString(),
                Is.EqualTo("Category deleted"));
        }
    }
}
