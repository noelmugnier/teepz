using Teeps.Web.Api.Domain.Common;

namespace Teeps.Web.Api.Domain;

public class PostVote
{
    private PostVote(){}
    public PostVote(Post post, CurrentUser currentUser, int value)
    {
        if (value is < -1 or > 1)
            throw new InvalidOperationException("post.vote.invalid.value");
        
        Value = value;
        PostId = post.Id;
        UserId = currentUser.Id;
        CreatedOn = DateTime.UtcNow;
    }

    public long PostId { get; private set; }
    public long UserId { get; private set; }
    public int Value { get; private set; }
    public DateTime CreatedOn { get; private set; }
}