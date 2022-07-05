namespace Teeps.Web.Api.Application.Post;

public record CommentDto(string Content, DateTimeOffset CreatedOn, DateTimeOffset UpdatedOn, AuthorDto Author);