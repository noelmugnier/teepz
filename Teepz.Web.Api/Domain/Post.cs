using System.Collections.ObjectModel;
using Teeps.Web.Api.Domain.Common;
using Teeps.Web.Api.Infrastructure;

namespace Teeps.Web.Api.Domain;

public class Post
{
    private string _content;
    private List<Tag> _tags = new List<Tag>();
    private List<PostVote> _votes = new List<PostVote>();
    private List<PostLike> _likes = new List<PostLike>();
    private List<PostComment> _comments = new List<PostComment>();

    private Post(){}
    public Post(string content, IEnumerable<Tag> tags, CurrentUser currentUser)
    {
        Content = content;
        Tags = tags;
        UserId = currentUser.Id;
        CreatedOn = DateTime.UtcNow;
        UpdatedOn = DateTime.UtcNow;
    }

    public long Id { get; private set; }
    public long UserId { get; private set; }

    public DateTime CreatedOn { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    public string Content
    {
        get => _content;
        set
        {
            _content = value;
            UpdatedOn = DateTime.UtcNow;
        }
    }

    public IEnumerable<Tag> Tags
    {
        get => _tags;
        set
        {
            _tags = value == null ? new List<Tag>() : value.ToList();
            UpdatedOn = DateTime.UtcNow;
        }
    }

    public virtual IReadOnlyCollection<PostVote> Votes => _votes.AsReadOnly();
    public virtual IReadOnlyCollection<PostLike> Likes => _likes.AsReadOnly();
    public virtual IReadOnlyCollection<PostComment> Comments => _comments.AsReadOnly();
    public virtual ApplicationUser User { get; private set; }
}