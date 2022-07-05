namespace Teeps.Web.Api.Application.Post;

public record PostDetailsDto(long Id, string Content, int Score, int Likes, int VotesCount, int CommentsCount, IEnumerable<string> Tags, DateTimeOffset CreatedOn, DateTimeOffset UpdatedOn, AuthorDto Author);