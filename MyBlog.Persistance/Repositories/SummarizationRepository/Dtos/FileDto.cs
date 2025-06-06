namespace MyBlog.Persistance.Repositories.SummarizationRepository.Dtos;

public class FileDto
{
    public required byte[] Bytes { get; set; }
    public required string FileName { get; set; }
}