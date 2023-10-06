using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models.Post;
using MyBlog.BlazorApp.Services.Post;

namespace MyBlog.BlazorApp.Pages.Posts
{
    public partial class EditPost
    {
        [Inject]
        private IPostService postService { get; set; } = null!;
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string Username { get; set; }

        IBrowserFile file;
        byte[]? blob = null;
        Dictionary<string, byte[]> images = new Dictionary<string, byte[]>();

        public PostVm _post = new PostVm();

        public EditPostVm _editPost = new EditPostVm();

        protected override async Task OnParametersSetAsync()
        {
            _post = new PostVm();
            _editPost = new EditPostVm();
            var apiPost = await postService.GetPostAsync(Id);
            var result = await AuthStateProvider.GetAuthenticationStateAsync();
            Username = result.User.Identity!.Name!;
            if (string.IsNullOrEmpty(Username) || apiPost!.AuthorsName != Username)
            {
                NavigationManager.NavigateTo("/");
            }
            _post = apiPost!;
            _editPost.Id = _post.Id;
            _editPost.Title = _post.Title;
            _editPost.Text = _post.Text;
            _editPost.AuthorName = Username!;
            images = new Dictionary<string, byte[]>();
            if (_post.Images64s != null && _post.Images64s.Any())
            {
                foreach (var img in _post.Images64s)
                {
                    var arr = img.Split(new char[] { ',' });
                    var base64 = arr[arr.Length-1];
                    var raw = Convert.FromBase64String(base64);
                    images.Add(img, raw);
                }
            }

            file = null;
        }

        async Task HandleEditPost()
        {
            await postService.EditPostAsync(_editPost, images.Values);

            NavigationManager.NavigateTo($"/post/{_editPost.Id}");

        }
        async Task OnInputFileChange(InputFileChangeEventArgs args)
        {
            file = args.File;
            var buffers = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffers);
            string imageType = file.ContentType;
            var imgUrl = $"data:{imageType};base64,{Convert.ToBase64String(buffers)}";
            images.Add(imgUrl, buffers);
        }
        void HandleDeleteImg(string imgKey)
        {
            images.Remove(imgKey);
        }
    }
}
