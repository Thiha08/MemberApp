namespace MemberApp.Data.Infrastructure.Services.Abstract
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, string username, string[] roles);
        bool IsTokenValid(string key, string issuer, string token);
    }
}
