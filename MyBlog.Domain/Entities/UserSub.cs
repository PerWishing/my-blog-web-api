namespace MyBlog.Domain.Entities;

public class UserSub
{
    public UserSub(string username, int postId)
    {
        Username = username;
        PostId = postId;
    }
    
    public int Id { get; set; }
    public string Username { get; set; }
    public int PostId { get; set; }
}