using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Persistance.Repositories.UserRepository
{
    public class SetRefreshTokenRequest
    {
        public string Username { get; set;} = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime? RefreshTokenExpiryTime { get; set; } = null;
    }
}
