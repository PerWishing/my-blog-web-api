namespace MyBlog.Persistance.ExternalFeatures.SummarizationModelApis.Dtos;

public class SumApiResponseDto
{
    public required string Topic { get; set; }
    public required string Summarized { get; set; }
}