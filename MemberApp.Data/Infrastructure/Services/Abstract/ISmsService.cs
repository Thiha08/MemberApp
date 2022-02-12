using System.Threading.Tasks;

namespace MemberApp.Data.Infrastructure.Services.Abstract
{
    public interface ISmsService
    {
        Task SendSMSAsync(string to, string message);
    }
}
