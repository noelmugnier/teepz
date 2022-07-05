namespace Teeps.Web.Api.Application.Settings;

public record JwtSettingsOptions : JwtSettings
{
    public static string Position = "Jwt";
}

public record JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
}