using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Persistance.Repositories.PostRepository
{
    public class GetPostResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime PublishDate { get; set; }
        public string AuthorsName { get; set; } = null!;
        
        public IEnumerable<int>? SumIds { get; set; }
    }
}
