using Microsoft.AspNetCore.Http;

namespace BdTracker.Gateway.Infrastructures;

public class HttpClientFactoryFacade : IHttpClientFactoryFacade
{
    private readonly IHttpClientFactory _internalHttpClientFactory;

    public HttpClientFactoryFacade(IHttpClientFactory httpClientFactory)
    {
        _internalHttpClientFactory = httpClientFactory;
    }

    public HttpClient CreateClient()
    {
        var client = _internalHttpClientFactory.CreateClient();

        return client;
    }
}