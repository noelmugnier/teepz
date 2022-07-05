using Teeps.Web.Api.Domain.Common;

namespace Teeps.Web.Api.Domain;

public class PostLike
{
    private PostLike(){}
    public PostLike(Post post, CurrentUser currentUser)
    {
        PostId = post.Id;
        Value = 1;
        UserId = currentUser.Id;
        CreatedOn = DateTime.UtcNow;
    }
    
    public long PostId { get; private set; }
    public long UserId { get; private set; }
    public int Value { get; private set; }
    public DateTime CreatedOn { get; private set; }
}