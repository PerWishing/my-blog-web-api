namespace MyBlog.Persistance.Repositories.SummarizationRepository.Dtos;

public class CreateSumRequest
{
    public int PostId { get; set; }
    public string? InputText { get; set; }

    public string? AuthorsName { get; set; }
}