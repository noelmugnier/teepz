namespace Teeps.Web.Api.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string code) : base(code)
    {
    }
}