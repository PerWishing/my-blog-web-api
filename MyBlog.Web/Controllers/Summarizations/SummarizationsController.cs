using Microsoft.AspNetCore.Mvc;
using MyBlog.Persistance.Database;
using MyBlog.Persistance.Repositories.ImageRepository;
using MyBlog.Persistance.Repositories.PostRepository;
using MyBlog.Persistance.Repositories.SummarizationRepository;
using MyBlog.Persistance.Repositories.SummarizationRepository.Dtos;
using MyBlog.Web.Dto.Summarizations;

namespace MyBlog.Web.Controllers.Summarizations;

[ApiController]
[Route("/api/sum")]
public class SummarizationsController : ControllerBase
{
    private readonly SummarizationManager summarizationManager;
    private readonly IPostManager postManager;
    private readonly ApplicationDbContext context;

    public SummarizationsController(
        SummarizationManager summarizationManager,
        IPostManager postManager,
        ApplicationDbContext context)
    {
        this.summarizationManager = summarizationManager;
        this.postManager = postManager;
        this.context = context;
    }

    [Route("create")]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromForm] CreatePostRequest request, IEnumerable<IFormFile>? images)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }

        ModelState.Remove("AuthorsName");
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        request.AuthorsName = User.Identity.Name!;

        await context.Database.BeginTransactionAsync();
        var postId = 0;
        try
        {
            postId = await postManager.CreateAsync(request);

            var formFiles = images?.ToList();
            if (formFiles != null && formFiles.Any() && postId > 0)
            {
                var file = formFiles.First();

                if (Path.GetExtension(file.FileName) != ".xlsx")
                {
                    return BadRequest("Поддерживаются только xlsx файлы");
                }

                summarizationManager.CreateSummarizationFromFile(
                    postId,
                    file,
                    User.Identity!.Name!);
            }
            else
            {
                await summarizationManager.DoSimpleSummarization(postId, request.Text!, User.Identity.Name!);
            }

            await context.Database.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await context.Database.RollbackTransactionAsync();
            throw;
        }

        if (postId > 0)
            return Ok(postId);
        else
            return BadRequest();
    }

    [Route("download-input-by-post")]
    [HttpPost]
    public IActionResult DownloadInputFile([FromBody]int postId)
    {
        var file = summarizationManager.DownloadSummarizationInput(postId);

        return File(
            file.Bytes,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.sheet",
            file.FileName);
    }
}