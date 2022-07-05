namespace Teeps.Web.Api.Application.Post;

public record PostDto(long Id, string Content, int Score, int Likes, int VotesCount, int CommentsCount, bool HasMoreContent, IEnumerable<string> Tags, DateTimeOffset CreatedOn, DateTimeOffset UpdatedOn, AuthorDto Author);