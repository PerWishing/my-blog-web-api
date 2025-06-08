using Microsoft.AspNetCore.Mvc;
using MyBlog.BlazorApp.Models.Post;
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

    [Route("{sumId:int}")]
    [HttpGet]
    public async Task<IActionResult> GetSummarization(int sumId)
    {
        var result = await summarizationManager.GetSummarizationAsync(sumId);
        return Ok(result);
    }
    
    [Route("create-project")]
    [HttpPost]
    public async Task<ActionResult<int>> CreateProject([FromForm] CreatePostRequest request)
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

        var postId = await postManager.CreateAsync(request);

        if (postId > 0)
            return Ok(postId);
        else
            return BadRequest();
    }

    [Route("create-sum")]
    [HttpPost]
    public async Task<ActionResult<int>> CreateSum([FromForm] CreateSumRequest request, IEnumerable<IFormFile>? files)
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
        var postId = request.PostId;
        try
        {
            var formFiles = files?.ToList();
            if (formFiles != null && formFiles.Any())
            {
                var file = formFiles.First();

                if (Path.GetExtension(file.FileName) != ".xlsx" && Path.GetExtension(file.FileName) != ".docx")
                {
                    return BadRequest("Поддерживаются только xlsx и docx файлы");
                }

                summarizationManager.CreateSummarizationFromFile(
                    postId,
                    file,
                    User.Identity!.Name!);
            }
            else
            {
                await summarizationManager.DoSimpleSummarization(postId, request.InputText!, User.Identity.Name!);
            }

            await context.Database.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await context.Database.RollbackTransactionAsync();
            throw;
        }
        
        return Ok(postId);
    }


    [Route("download-input-by-post")]
    [HttpPost]
    public IActionResult DownloadInputFile([FromBody] int sumId)
    {
        var file = summarizationManager.DownloadSummarizationInput(sumId);

        return File(
            file.Bytes,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.sheet",
            file.FileName);
    }

    [Route("download-output-by-post")]
    [HttpPost]
    public IActionResult DownloadOutputFile([FromBody] int sumId)
    {
        var file = summarizationManager.DownloadSummarizationOutput(sumId);

        return File(
            file.Bytes,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.sheet",
            file.FileName);
    }
}