using Microsoft.AspNetCore.Mvc;
using MyBlog.Persistance.Repositories.SummarizationRepository;
using MyBlog.Persistance.Repositories.SummarizationRepository.Dtos;
using MyBlog.Web.Dto.Summarizations;

namespace MyBlog.Web.Controllers.Summarizations;

[ApiController]
[Route("/api/sum")]
public class SummarizationsController : ControllerBase
{
    private readonly SummarizationManager summarizationManager;

    public SummarizationsController(
        SummarizationManager summarizationManager)
    {
        this.summarizationManager = summarizationManager;
    }
    
    [HttpPost("simple")]
    public async Task<IActionResult> DoSimpleSummarization(
        DoSummarizationDto dto)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }
        
        var result = await summarizationManager.DoSimpleSummarization(dto.Text, User.Identity!.Name!);
        
        return Ok(result);
    }
}