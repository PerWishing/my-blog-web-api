using System.Text;
using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models.Post;
using MyBlog.BlazorApp.Models.Summarizations;
using MyBlog.BlazorApp.Services.Post;

namespace MyBlog.BlazorApp.Pages.Summarizations;

public partial class SummarizationCard
{
    [Inject] private IPostService postService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Parameter] public int SumId { get; set; }

    public string? ShortInputText { get; set; }

    public SumVm _sum = new SumVm();

    protected override async Task OnParametersSetAsync()
    {
        var sum = await postService.GetSumAsync(SumId);

        if (sum != null)
        {
            _sum = sum;
        }

        var sb = new StringBuilder();

        if (_sum.InputText != null)
        {
            var strArray = _sum.InputText.Split(" ");

            var count = strArray.Length < 6 ? strArray.Length : 5;

            foreach (var i in Enumerable.Range(0, count - 1))
            {
                sb.Append(strArray[i]);
                sb.Append(" ");
            }

            sb.Append("...");
            
            ShortInputText = sb.ToString();
        }
    }
}