﻿using MyBlog.BlazorApp.Models.Post;
using System.Text;
using System.Text.Json;
using MyBlog.BlazorApp.Models.Summarizations;

namespace MyBlog.BlazorApp.Services.Post
{
    public class PostService : IPostService
    {
        private HttpClient httpClient;

        public PostService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<PostVm?> GetPostAsync(int id)
        {
            try
            {
                var apiResponse = await httpClient.GetStreamAsync($"api/post/{id}");

                var postVm = await JsonSerializer.DeserializeAsync<PostVm>(apiResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
                var imgsSrcs = new List<string>();
                foreach (var img in postVm.Images64s)
                {
                    var imgStr = img.Replace("\"", "");
                    imgsSrcs.Add("data:image/png;base64," + imgStr);
                }

                postVm.Images64s = imgsSrcs;
                return postVm;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<PostsPageVm?> GetAllPostsAsync(string? search, int page = 1)
        {
            try
            {
                Stream apiResponse = null;
                if (string.IsNullOrEmpty(search))
                {
                    apiResponse = await httpClient.GetStreamAsync($"api/all-posts/{page}");
                }
                else
                {
                    apiResponse = await httpClient.GetStreamAsync($"api/all-posts/{search}/{page}");
                }

                var postsPage = await JsonSerializer.DeserializeAsync<PostsPageVm>(apiResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return postsPage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<PostsPageVm?> GetUserPostsAsync(string username, int page = 1)
        {
            try
            {
                var apiResponse = await httpClient.GetStreamAsync($"api/posts/{username}/{page}");

                var postsPage = await JsonSerializer.DeserializeAsync<PostsPageVm>(apiResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return postsPage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<PostsPageVm?> GetUserSavedPostsAsync(string username, int page = 1)
        {
            try
            {
                var apiResponse = await httpClient.GetStreamAsync($"api/saved-posts/{username}/{page}");

                var postsPage = await JsonSerializer.DeserializeAsync<PostsPageVm>(apiResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return postsPage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> IsSavedPostAsync(int id)
        {
            try
            {
                var apiResponse = await httpClient.GetStreamAsync($"api/post/is-saved/{id}");
                var isSaved = await JsonSerializer.DeserializeAsync<bool>(apiResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
                if (isSaved)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<string?> SavePostAsync(int id)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(id), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PostAsync($"api/save-post/{id}", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }

                return "Can't save post.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> DeleteSavedPostAsync(int id)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(id), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.DeleteAsync($"api/delete-saved/{id}");

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }

                return "Can't save post.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<ProjectSubsVm?> GetProjectSubsAsync(int postId)
        {
            try
            {
                var apiResponse = await httpClient.GetStreamAsync($"api/sum/proj-subs/{postId}");

                var projectSubs = await JsonSerializer.DeserializeAsync<ProjectSubsVm>(apiResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return projectSubs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<SumVm?> GetSumAsync(int sumId)
        {
            try
            {
                var apiResponse = await httpClient.GetStreamAsync($"api/sum/{sumId}");

                var sumVm = await JsonSerializer.DeserializeAsync<SumVm>(apiResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return sumVm;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<int?> CreateSummarizationAsync(CreateSumVm sum, FileVm? files)
        {
            try
            {
                var multipartContent = new MultipartFormDataContent();
                if (sum.InputText != null)
                {
                    multipartContent.Add(new StringContent(sum.InputText), String.Format("\"{0}\"", "InputText"));
                }

                multipartContent.Add(new StringContent(sum.PostId.ToString()), String.Format("{0}", "PostId"));
                multipartContent.Add(new StringContent(""), String.Format("\"{0}\"", "AuthorsName"));

                if (files != null)
                {
                    multipartContent.Add(new ByteArrayContent(files.File), "files", $"{files.FileName}");
                }

                var apiResponse = await httpClient.PostAsync("api/sum/create-sum", multipartContent);

                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseBody = await apiResponse.Content.ReadAsStreamAsync();

                    var postId = await JsonSerializer.DeserializeAsync<int>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    return postId;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<int?> CreateProjectAsync(CreatePostVm post)
        {
            try
            {
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new StringContent(post.Title), String.Format("\"{0}\"", "Title"));
                multipartContent.Add(new StringContent(post.Text), String.Format("\"{0}\"", "Text"));
                multipartContent.Add(new StringContent(""), String.Format("\"{0}\"", "AuthorsName"));

                var apiResponse = await httpClient.PostAsync("api/sum/create-project", multipartContent);

                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseBody = await apiResponse.Content.ReadAsStreamAsync();

                    var postId = await JsonSerializer.DeserializeAsync<int>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    return postId;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> EditPostAsync(EditPostVm post, IEnumerable<byte[]>? images)
        {
            try
            {
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(new StringContent(post.Id.ToString()), String.Format("\"{0}\"", "Id"));
                multipartContent.Add(new StringContent(post.Title), String.Format("\"{0}\"", "Title"));
                multipartContent.Add(new StringContent(post.Text), String.Format("\"{0}\"", "Text"));
                multipartContent.Add(new StringContent(post.AuthorName), String.Format("\"{0}\"", "username"));

                if (images != null && images.Any())
                {
                    foreach (var img in images)
                    {
                        multipartContent.Add(new ByteArrayContent(img), "images", "img.png");
                    }
                }

                var apiResponse = await httpClient.PutAsync("api/edit-post", multipartContent);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> DeletePostAsync(int id)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(id), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.DeleteAsync($"api/delete-post/{id}");

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }

                return "Can't delete post.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


        public async Task<Stream?> DownloadInputSummarizationAsync(int sumId)
        {
            var itemJson = new StringContent(JsonSerializer.Serialize(sumId), Encoding.UTF8, "application/json");
            var apiResponse = await httpClient.PostAsync($"api/sum/download-input-by-post", itemJson);

            var responseBody = await apiResponse.Content.ReadAsStreamAsync();

            return apiResponse.IsSuccessStatusCode ? responseBody : null;
        }

        public async Task<Stream?> DownloadOutputSummarizationAsync(int sumId)
        {
            var itemJson = new StringContent(JsonSerializer.Serialize(sumId), Encoding.UTF8, "application/json");
            var apiResponse = await httpClient.PostAsync($"api/sum/download-output-by-post", itemJson);

            var responseBody = await apiResponse.Content.ReadAsStreamAsync();

            return apiResponse.IsSuccessStatusCode ? responseBody : null;
        }
    }
}