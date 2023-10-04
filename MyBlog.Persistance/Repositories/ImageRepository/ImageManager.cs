using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Persistance.Database;
using MyBlog.Persistance.Identity;
using MyBlog.Domain.Entities;
using System.Xml.Linq;
using System.Linq;
using System.IO;

namespace MyBlog.Persistance.Repositories.ImageRepository
{
    public partial class ImageManager : IImageManager
    {
        private readonly ApplicationDbContext context;

        static string slnPath = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
        static string webRootPath = Path.Combine(slnPath, "MyBlog.Web", "wwwroot");
        static string postsFolder = "postsImages";

        public ImageManager(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<string>> GetImages64ByPostIdAsync(int id, bool firstOnly = false)
        {
            List<PostImage> queryResult = new List<PostImage>();
            if (firstOnly)
            {
                var img = await context.PostImages.Include(x => x.Post).FirstOrDefaultAsync(x => x.Post.Id == id);
                if (img != null)
                {
                    queryResult.Add(img);
                }
            }
            else
            {
                queryResult = await context.PostImages.Include(x => x.Post).Where(x => x.Post.Id == id).ToListAsync();
            }

            var images64s = new List<string>();
            if (queryResult.Any())
            {
                foreach (var imageName in queryResult)
                {
                    var path = Path.Combine(webRootPath, postsFolder, imageName.ImageName);


                    if (File.Exists(path))
                    {
                        using (var imageFileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete))
                        {
                            //imageFileStream = File.OpenRead(path);
                            try
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    imageFileStream.CopyTo(memoryStream);

                                    byte[] byteImage = memoryStream.ToArray();
                                    var base64String = Convert.ToBase64String(byteImage);
                                    images64s.Add(base64String);
                                }
                            }
                            catch (Exception ex)
                            {
                                await Console.Out.WriteLineAsync(ex.Message);
                            }
                        }
                    }
                }
            }

            return images64s;
        }

        public async Task<bool> DeletePostImageAsync(string imagename)
        {
            var image = await context.PostImages.FirstOrDefaultAsync(x => x.ImageName == imagename);

            if (image != null)
            {
                //delete image from /wwwroot/postsImages
                var imagePath = Path.Combine(webRootPath + "/postsImages/" + image.ImageName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
                //delete image record from db
                context.PostImages.Remove(image);

                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UploadPostImagesAsync(IEnumerable<UploadImageRequest> requests)
        {
            var post = context.Posts.Where(x => x.Id == requests.First().PostId).FirstOrDefault();

            if (post == null)
            {
                return false;
            }

            //Save image to wwwroot/postsImages
            string wwwRootPath = webRootPath;
            var imgs = new List<PostImage>();
            foreach (var request in requests)
            {
                string fileName = "Post-" + requests.First().PostId;
                string extention = ".png";
                fileName = fileName + "_" + DateTime.Now.ToString("yymmssfff") + extention;
                string path = Path.Combine(wwwRootPath + "/postsImages/" + fileName);
                //Resize by ImageSharp lib

                try
                {
                    using (var image = Image.Load(request.ImageFile.OpenReadStream()))
                    {
                        var b = image.Bounds;
                        var prop = b.Width / b.Height;
                        if (b.Height > 400)
                        {
                            image.Mutate(x => x.Resize((400 * prop), 400));
                        }
                        image.Save(path);
                        image.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }

                var postImage = new PostImage
                {
                    ImageName = fileName,
                    Post = post
                };

                imgs.Add(postImage);
            }
            await context.PostImages.AddRangeAsync(imgs);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostImagesAsync(int id)
        {
            var imagesQuery = await context.Posts.Include(x => x.Images)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (imagesQuery != null && imagesQuery.Images != null)
            {
                foreach (var image in imagesQuery.Images)
                {

                    //delete image from /wwwroot/postsImages
                    var imagePath = Path.Combine(webRootPath + "/postsImages/" + image.ImageName);
                    if (File.Exists(imagePath))
                    {
                        try
                        {
                            File.Delete(imagePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    //delete image record from db
                    context.PostImages.Remove(image);
                }
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<string> GetAvatarAsync(string username)
        {
            var avatarQuery = await context.Avatars
                .Include(x => x.UserProfile)
                .Where(x => x.UserProfile.UserName == username)
                .FirstOrDefaultAsync();

            string avatarName = "avatarSample.png";

            if (avatarQuery != null && !avatarQuery.UserProfile.IsBlocked)
            {
                avatarName = avatarQuery.ImageName;
            }
            var path = Path.Combine(webRootPath, "avatars", $"{avatarName}");
            var imageFileStream = File.OpenRead(path);

            using (var memoryStream = new MemoryStream())
            {
                imageFileStream.CopyTo(memoryStream);

                byte[] byteImage = memoryStream.ToArray();
                var base64String = Convert.ToBase64String(byteImage);
                return base64String;
            }
        }

        public async Task<bool> UploadUserAvatarAsync(UploadAvatarRequest request)
        {
            var avatarQuery = await context.Avatars.Include(x => x.UserProfile)
                .Where(x => x.UserProfile.UserName == request.Username).FirstOrDefaultAsync();
            if (avatarQuery != null)
            {
                //delete image from /wwwroot/Image
                var imagePath = Path.Combine(webRootPath + "/avatars/" + avatarQuery.ImageName);
                if (File.Exists(imagePath))
                {
                    try
                    {
                        File.Delete(imagePath);
                    }
                    catch (Exception ex)
                    {
                        await Console.Out.WriteLineAsync(ex.Message);
                    }
                }
                //delete image record from db
                context.Avatars.Remove(avatarQuery);
            }

            //Save image to wwwroot/avatars
            string wwwRootPath = webRootPath;
            string fileName = request.Username;
            string extention = Path.GetExtension(request.ImageFile.FileName);
            fileName = fileName + "_" + DateTime.Now.ToString("yymmssfff") + extention;
            string path = Path.Combine(wwwRootPath + "/avatars/" + fileName);
            //Resize by ImageSharp lib
            using (var image = SixLabors.ImageSharp.Image.Load(request.ImageFile.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(180, 180));
                image.Save(path);
            }

            //Save image to wwwroot/avatars
            //string wwwRootPath = hostEnvironment.WebRootPath;
            //string fileName = Path.GetFileNameWithoutExtension(request.ImageName);
            //string extention = Path.GetExtension(request.ImageName);
            //request.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extention;
            //string path = Path.Combine(wwwRootPath + "/avatars/" + fileName);
            //using (var fileStream = new FileStream(path, FileMode.Create))
            //{
            //    await request.ImageFile.CopyToAsync(fileStream);
            //}

            var userAvatar = new UserAvatar
            {
                ImageName = fileName,
                UploadDate = DateTime.Now,
            };

            var userProfile = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == request.Username);

            if (userProfile != null)
            {
                userAvatar.UserProfile = userProfile;
            }

            //Insert image record into db
            context.Avatars.Add(userAvatar);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAvatarAsync(string username)
        {
            var avatarQuery = await context.Avatars.Include(x => x.UserProfile)
                .Where(x => x.UserProfile.UserName == username).FirstOrDefaultAsync();
            if (avatarQuery != null)
            {
                //delete image from /wwwroot/Image
                var imagePath = Path.Combine(webRootPath + "/avatars/" + avatarQuery.ImageName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
                //delete image record from db
                context.Avatars.Remove(avatarQuery);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
