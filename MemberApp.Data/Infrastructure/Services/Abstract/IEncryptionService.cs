namespace MemberApp.Data.Infrastructure.Services.Abstract
{
    public interface IEncryptionService
    {
        string CreateSalt();
      
        string EncryptPassword(string password, string salt);
    }
}
