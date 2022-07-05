using Teeps.Web.Api.Domain.Common;

namespace Teeps.Web.Api.Domain;

public enum CommentKind
{
    PostComment,
    CommentReply
}

public class PostComment : Comment
{
    private PostComment(){}
    public PostComment(Post post, string value, CurrentUser currentUser)
        : base(CommentKind.PostComment, value, currentUser)
    {
        PostId = post.Id;
    }
    public long PostId { get; private set; }
}