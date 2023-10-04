using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models.Comments;
using MyBlog.BlazorApp.Services.Comment;
using MyBlog.BlazorApp.Services.Post;
using System.Xml.Linq;

namespace MyBlog.BlazorApp.Pages.Comments
{
    public partial class Comment
    {
        [Inject]
        private ICommentService commentService { get; set; } = null!;

        [CascadingParameter(Name = "IsAuth")]
        public bool IsAuth { get; set; }
        [CascadingParameter(Name = "ParentComments")]
        public Comments Parent { get; set; } = null!;
        [CascadingParameter(Name = "Username")]
        public string Username { get; set; }

        [Parameter]
        public CommentDto CommentModel { get; set; } = null!;
        public CommentDto _comment = new CommentDto(); 

        protected override async Task OnParametersSetAsync()
        {
            _comment = new CommentDto();
            _comment = CommentModel;
            _comment.IsLiked = await commentService.IsLikedAsync(_comment.Id);
            await Console.Out.WriteLineAsync($"Check {_comment.Id}");
        }

        async Task DeleteComAsync()
        {
            await commentService.DeleteAsync(_comment.Id);
            await Console.Out.WriteLineAsync("DEL COMPLITED");
            await Parent.RefreshComponentAsync();
        }

        async Task LikeComAsync()
        {
            await commentService.LikeAsync(_comment.Id);
            _comment.IsLiked = true;
            _comment.LikesCount += 1;
        }
        async Task UnlikeComAsync()
        {
            await commentService.UnlikeAsync(_comment.Id);
            _comment.IsLiked = false;
            _comment.LikesCount -= 1;
        }
    }
}
