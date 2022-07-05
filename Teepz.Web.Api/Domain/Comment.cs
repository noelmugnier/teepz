using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Domain;

public abstract class Comment
{
    private string _value;

    protected Comment(){}

    protected Comment(CommentKind kind, string value, CurrentUser currentUser)
    {
        Kind = kind;
        Value = value;
        UserId = currentUser.Id;
        CreatedOn = DateTime.UtcNow;
        UpdatedOn = DateTime.UtcNow;
    }

    public long Id { get; private set; }
    public long UserId { get; private set; }
    public CommentKind Kind { get; private set; }

    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            UpdatedOn = DateTime.UtcNow;
        }
    }

    public DateTime CreatedOn { get; private set; }
    public DateTime UpdatedOn { get; private set; }
    public virtual ApplicationUser User { get; private set; }
}