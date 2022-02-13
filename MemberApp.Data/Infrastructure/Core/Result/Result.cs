using MemberApp.Data.Infrastructure.Core.Extensions;

namespace MemberApp.Data.Infrastructure.Core.Result
{
    public class Result : IResult
    {
        public Result() { }

        public ResultStatus Status { get; set; }

        public string Message { get; set; }

        public bool IsSuccess => Status == ResultStatus.Ok;

        private Result(ResultStatus status)
        {
            Status = status;
        }

        public static Result Ok(string message = null)
        {
            return new Result(ResultStatus.Ok) { Message = message ?? ResultStatus.Ok.ToDescription() };
        }

        public static Result BadRequest(string message = null)
        {
            return new Result(ResultStatus.BadRequest) { Message = message ?? ResultStatus.BadRequest.ToDescription() };
        }

        public static Result InternalServerError(string message = null)
        {
            return new Result(ResultStatus.InternalServerError) { Message = message ?? ResultStatus.InternalServerError.ToDescription() };
        }

    }

    public class Result<T> : Result
    {
        protected Result(T data)
        {
            Data = data;
        }

        private Result(ResultStatus status)
        {
            Status = status;
        }

        public T Data { get; set; }

        public static implicit operator T(Result<T> result) => result.Data;
        public static implicit operator Result<T>(T value) => Ok(value);

        public PagedResult<T> ToPagedResult(PagedInfo pagedInfo)
        {
            var pagedResult = new PagedResult<T>(pagedInfo, Data)
            {
                Status = Status,
                Message = Message
            };

            return pagedResult;
        }

        public static Result<T> Ok(T data, string message = null)
        {
            return new Result<T>(data)
            {
                Status = ResultStatus.Ok,
                Message = message ?? ResultStatus.Ok.ToDescription()
            };
        }

        public static new Result<T> BadRequest(string message = null)
        {
            return new Result<T>(ResultStatus.BadRequest) { Message = message ?? ResultStatus.BadRequest.ToDescription() };
        }

        public static new Result<T> InternalServerError(string message = null)
        {
            return new Result<T>(ResultStatus.InternalServerError) { Message = message ?? ResultStatus.InternalServerError.ToDescription() };
        }
    }
}
