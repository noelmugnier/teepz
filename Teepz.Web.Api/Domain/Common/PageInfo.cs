namespace Teeps.Web.Api.Domain.Common;

public record PageInfo(int Page, int Take)
{
    public int Skip => Page * Take;
}