namespace MemberApp.Data.Infrastructure.Core.Result
{
    public interface IResult
    {
        ResultStatus Status { get; }

        string Message { get; set; }

        bool IsSuccess { get; }
    }
}
