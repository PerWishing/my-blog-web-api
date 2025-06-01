using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using MyBlog.Persistance.Repositories.SummarizationRepository.Dtos;

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

        var requestDto = new SumApiRequestDto
        {
            Text = inputText
        };
        
        var json = JsonContent.Create(requestDto);
        
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