using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Domain.Entities.Summarizations;

namespace MyBlog.Domain.Entities
{
    public class UserProfile
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public DateTime? YearOfBirth { get; set; }
        public string? AboutMyself { get; set; }
        public IEnumerable<Post>? Posts { get; set; }
        public UserAvatar? Avatar { get; set; }
        public bool IsBlocked { get; set; }
        
        public IEnumerable<Summarization>? Summarizations { get; set; }
    }
}
