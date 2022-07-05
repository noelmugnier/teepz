using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Teeps.Web.Api.Domain;

namespace Teeps.Web.Api.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        // {
        //     // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
        //     // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
        //     // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
        //     // use the DateTimeOffsetToBinaryConverter
        //     // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
        //     // This only supports millisecond precision, but should be sufficient for most use cases.
        //     foreach (var entityType in builder.Model.GetEntityTypes())
        //     {
        //         var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
        //                                                                        || p.PropertyType == typeof(DateTimeOffset?));
        //         foreach (var property in properties)
        //         {
        //             builder
        //                 .Entity(entityType.Name)
        //                 .Property(property.Name)
        //                 .HasConversion(new DateTimeOffsetToBinaryConverter());
        //         }
        //     }
        // }
        
        var userBuilder = builder.Entity<ApplicationUser>();
        userBuilder.OwnsMany(p => p.Tags, dd =>
        {
            dd.Property(c => c.Value)
                .HasColumnName("Value");
            
            dd.WithOwner().HasForeignKey("UserId");
            dd.Property<long>("Id");
            dd.HasKey("Id");
            
            dd.ToTable("UserTags");
        });

        userBuilder.Navigation(c => c.Tags)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_tags");

        userBuilder.Property(c => c.Company)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        userBuilder.Property(c => c.Position)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        userBuilder.Property(c => c.Fullname)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        userBuilder.ToTable("Users");
        
        
        builder.Entity<IdentityUserRole<long>>()
            .ToTable("UserRoles");
        
        builder.Entity<IdentityUserClaim<long>>()
            .ToTable("UserClaims");
        
        builder.Entity<IdentityUserLogin<long>>()
            .ToTable("UserLogins");
        
        builder.Entity<IdentityUserToken<long>>()
            .ToTable("UserTokens");

        builder.Entity<ApplicationRole>()
            .ToTable("Roles");
        
        builder.Entity<IdentityRoleClaim<long>>()
            .ToTable("RoleClaims");
        
        
        var postBuilder = builder.Entity<Post>();
        postBuilder.HasKey(p => p.Id);
        postBuilder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        postBuilder.HasMany(p => p.Votes)
            .WithOne()
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        postBuilder.Navigation(c => c.Votes)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_votes");
        
        postBuilder.HasMany(p => p.Likes)
            .WithOne()
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        postBuilder.Navigation(c => c.Likes)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_likes");
        
        postBuilder
            .HasMany(p => p.Comments)
            .WithOne()
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        postBuilder.Navigation(c => c.Comments)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_comments");

        postBuilder.OwnsMany(p => p.Tags, dd =>
        {
            dd.Property(c => c.Value)
                .HasColumnName("Value");
            
            dd.WithOwner().HasForeignKey("PostId");
            dd.Property<long>("Id");
            dd.HasKey("Id");
            
            dd.ToTable("PostTags");
        });
        
        postBuilder.Navigation(c => c.Tags)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_tags");
        
        postBuilder.ToTable("Posts");
        
        
        var commentBuilder = builder.Entity<Comment>();
        commentBuilder.HasKey(c => c.Id);
        commentBuilder
            .HasDiscriminator<CommentKind>("Kind")
            .HasValue<PostComment>(CommentKind.PostComment)
            .HasValue<CommentReply>(CommentKind.CommentReply);
        
        commentBuilder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        commentBuilder.ToTable("Comments");

        builder.Entity<PostComment>()
            .HasMany<CommentReply>()
            .WithOne()
            .HasForeignKey(c => c.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        var likeBuilder = builder.Entity<PostLike>();
        likeBuilder.HasOne<ApplicationUser>().WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
        likeBuilder.HasKey(c => new {c.PostId, c.UserId});
        likeBuilder.ToTable("PostLikes");
        
        
        var voteBuilder = builder.Entity<PostVote>();
        voteBuilder.HasOne<ApplicationUser>().WithMany().HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
        voteBuilder.HasKey(c => new {c.PostId, c.UserId});
        voteBuilder.ToTable("PostVotes");
    }
}