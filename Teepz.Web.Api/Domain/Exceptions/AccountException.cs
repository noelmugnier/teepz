namespace Teeps.Web.Api.Domain.Exceptions;

public class AccountException : Exception
{
    public AccountException(string code) : base(code)
    {
    }
}