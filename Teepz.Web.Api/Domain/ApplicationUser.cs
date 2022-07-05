using Microsoft.AspNetCore.Identity;
using Teeps.Web.Api.Domain;

namespace Teeps.Web.Api.Infrastructure;

public class ApplicationUser : IdentityUser<long>
{
    private List<Tag> _tags = new List<Tag>();
    private string _fullname;
    private string? _company;
    private string? _position;
    private ApplicationUser(){}
    
    public ApplicationUser(string email, string fullname) : base(email)
    {
        Fullname = fullname;
        CreatedOn = DateTime.UtcNow;
        UpdatedOn = DateTime.UtcNow;
    }

    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }

    public string Fullname
    {
        get => _fullname;
        set
        {
            _fullname = value;
            UpdatedOn = DateTime.UtcNow;
        }
    }

    public string? Company
    {
        get => _company;
        set
        {
            _company = value;
            UpdatedOn = DateTime.UtcNow;
        }
    }

    public string? Position
    {
        get => _position;
        set
        {
            _position = value;
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
}

public class ApplicationRole : IdentityRole<long>
{
}