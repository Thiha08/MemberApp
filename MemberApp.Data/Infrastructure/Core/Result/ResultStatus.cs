using System.ComponentModel;

namespace MemberApp.Data.Infrastructure.Core.Result
{
    public enum ResultStatus
    {
        [Description("Ok")]
        Ok = 200,

        [Description("BadRequest")]
        BadRequest = 400,

        [Description("InternalServerError")]
        InternalServerError = 500
    }
}
