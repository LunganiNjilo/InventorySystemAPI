public abstract class FunctionalTestBase : IDisposable
{
    protected HttpClient Client = default!;

    protected FunctionalTestBase()
    {
    }

    [SetUp]
    public void Setup()
    {
        Client = new HttpClient
        {
            BaseAddress = new Uri("http://inventorysystem-api:8080/")
        };
    }

    [TearDown]
    public void TearDown()
    {
        Client?.Dispose();
    }

    public void Dispose()
    {
        Client?.Dispose();
    }
}
