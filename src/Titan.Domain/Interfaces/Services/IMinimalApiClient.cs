namespace Titan.Domain.Interfaces.Services;

public interface IMinimalApiClient
{
    Task<TResponse> GetAsync<TResponse>(string endpoint, string username, string password);
    
    Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, string username, string password);
    
    Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, string username, string password);
    
    Task DeleteAsync(string endpoint, string username, string password);
}
