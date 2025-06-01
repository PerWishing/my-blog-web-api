namespace MyBlog.Persistance.Repositories.SummarizationRepository.Dtos;

public class SumApiResponseDto
{
    public required string Topic { get; set; }
    public required string Summarized { get; set; }
}