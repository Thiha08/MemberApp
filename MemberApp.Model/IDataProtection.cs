using MemberApp.Model.Constants;

namespace MemberApp.Model
{
    public interface IDataProtection
    {
        ProtectionStatus ProtectionStatus { get; set; }
    }
}
