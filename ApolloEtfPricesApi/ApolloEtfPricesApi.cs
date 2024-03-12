using ApolloEtfPricesWebApi.Database.Price;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ApolloEtfPricesApiAccessLibrary;

public class ApolloEtfPricesApi
{
    private HttpClient _httpClient;

    public ApolloEtfPricesApi(string url)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(url);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<HttpResponseMessage> AddPriceAsync(PriceObject priceObject, CancellationToken ct = default)
    {
        return await _httpClient.PutAsJsonAsync($"api/v1/addPrice", priceObject, ct);
    }
}
