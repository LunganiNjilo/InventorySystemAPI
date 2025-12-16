namespace Inventory.Tests.Infrastructure
{
    public class ApiListResponse<T>
    {
        public List<T> Result { get; set; } = new();
    }
}
