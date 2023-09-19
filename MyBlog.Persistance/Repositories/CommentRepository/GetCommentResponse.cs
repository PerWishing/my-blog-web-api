using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Persistance.Repositories.CommentRepository
{
    public class GetCommentResponse
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; } = null!;
        public string AuthorsName { get; set; } = null!;
        public DateTime Date { get; set; }
        public int LikesCount { get; set; }
    }
}
