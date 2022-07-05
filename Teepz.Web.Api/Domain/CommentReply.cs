using Teeps.Web.Api.Domain.Common;

namespace Teeps.Web.Api.Domain;

public class CommentReply : Comment
{
    private CommentReply(){}
    
    public CommentReply(Comment comment, string value, CurrentUser currentUser)
        : base(CommentKind.CommentReply, value, currentUser)
    {
        CommentId = comment.Id;
    }

    public long CommentId { get; private set; }
}