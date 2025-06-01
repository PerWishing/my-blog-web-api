using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using MyBlog.Persistance.ExternalFeatures.SummarizationModelApis.Dtos;

namespace MyBlog.Persistance.ExternalFeatures.SummarizationModelApis;

public class SummarizationModelHttpClient
{
    private readonly IHttpClientFactory clientFactory;
    private readonly IConfiguration configuration;

    public SummarizationModelHttpClient(
        IHttpClientFactory clientFactory,
        IConfiguration configuration)
    {
        this.clientFactory = clientFactory;
        this.configuration = configuration;
    }

    public async Task<SumApiResponseDto> GetSummarizationFromApi(string inputText)
    {
        var apiUrl = configuration["SummarizationApi"];
        
        var endpoint = "api/topandsum/";
        
        var httpClient = clientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        
        var requestDto = new SumApiRequestDto
        {
            text = inputText
        };
        
        var json = JsonContent.Create(requestDto);

        var byteArray = await json.ReadAsByteArrayAsync();
        
        json.Headers.ContentLength = byteArray.Length;
        
        var response = await httpClient.PostAsync(apiUrl + endpoint, json);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"SummarizationApi Error: {response.ReasonPhrase}");
        }
        
        var result = await response.Content.ReadFromJsonAsync<SumApiResponseDto>();

        if (result == null)
        {
            throw new HttpRequestException($"Запрос к api вернул пустой ответ");
        }
        
        return result;
    }
}