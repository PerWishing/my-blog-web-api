using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Persistance.Repositories.PostRepository
{
    public class CreatePostRequest
    {
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string AuthorsName { get; set; } = null!;
    }
}
