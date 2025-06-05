using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MyBlog.BlazorApp.Models.Post;
using MyBlog.BlazorApp.Services.Post;

namespace MyBlog.BlazorApp.Pages.Posts
{
    public partial class CreatePost
    {
        [Inject] private IPostService postService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        IBrowserFile file;
        byte[]? blob = null;
        Dictionary<string, byte[]> images = new Dictionary<string, byte[]>();
        public bool IsDisabled = false;

        public CreatePostVm _createPost = new CreatePostVm();

        protected override async Task OnParametersSetAsync()
        {
            images = new Dictionary<string, byte[]>();
            file = null;
        }

        async Task HandleCreatePost()
        {
            var id = await postService.CreateSummarizationPostAsync(_createPost, images.Values);
            if (id != null)
            {
                NavigationManager.NavigateTo($"/post/{id}");
            }
        }

        async Task OnInputFileChange(InputFileChangeEventArgs args)
        {
            file = args.File;
            var buffers = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffers);
            var fileName = file.Name;
            images.Add(fileName, buffers);
            IsDisabled = true ;
        }

        void HandleDeleteImg(string imgKey)
        {
            images.Remove(imgKey);
            IsDisabled = false;
        }
    }
}