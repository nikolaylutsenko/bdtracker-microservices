namespace BdTracker.Gateway.Infrastructures;

public interface IHttpClientFactoryFacade
{
    HttpClient CreateClient();
}