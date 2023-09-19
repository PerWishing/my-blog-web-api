using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Persistance.Repositories.CommentRepository
{
    public class CreateCommentRequest
    {
        public int PostId { get; set; }
        public string Text { get; set; } = null!;
        public string AuthorsName { get; set; } = null!;
    }
}
