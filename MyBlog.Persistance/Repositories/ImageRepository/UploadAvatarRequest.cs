﻿using Microsoft.AspNetCore.Http;

namespace MyBlog.Persistance.Repositories.ImageRepository
{
    public class UploadAvatarRequest
    {
        public string Username { get; set; } = null!;
        public IFormFile ImageFile { get; set; } = null!;
    }
}
