using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Services.Comment;
using MyBlog.BlazorApp.Models.Comment;

namespace MyBlog.BlazorApp.Pages.Comments
{
    public partial class Comments
    {
        [Inject]
        private ICommentService commentService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter]
        public int PostId { get; set; }
        [CascadingParameter(Name = "IsAuth")]
        public bool IsAuth { get; set; }
        [CascadingParameter(Name = "Username")]
        public string Username { get; set; }

        public IList<CommentVm> _comments = new List<CommentVm>();
        public CreateCommentVm _createComment = new CreateCommentVm();

        protected override async Task OnParametersSetAsync()
        {
            await ResetParametersAsync();

            var apiComments = await commentService.GetCommentsAsync(PostId);
            await Console.Out.WriteLineAsync("comms Gotten");
            if (apiComments != null && apiComments.Any())
            {
                _comments = apiComments;
            }

            await Console.Out.WriteLineAsync("comms Rendered");
        }

        async Task HandleCreateComment()
        {
            _createComment.PostId = PostId;
            await commentService.CreateCommentAsync(_createComment);
            await RefreshComponentAsync();
        }
        async Task ResetParametersAsync()
        {
            _createComment = new CreateCommentVm();
            _comments = new List<CommentVm>();
            await Console.Out.WriteLineAsync("Params reseted");
        }

        public async Task RefreshComponentAsync()
        {
            await OnParametersSetAsync();
            this.StateHasChanged();
        }
    }
}
