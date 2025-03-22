namespace BTM.Account.Domain.Abstractions
{

    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }

        public Result()
        {
            IsSuccess = true;
            ErrorMessage = string.Empty;
        }

        public Result(string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new Result();
        public static Result Failure(string errorMessage) => new Result(errorMessage);
    }

    public class Result<T> : Result
    {
        public T Data { get; private set; }

        public Result(T data) : base()
        {
            Data = data;
        }

        public Result(string errorMessage) : base(errorMessage) { }
        public static new Result<T> Success(T data) => new Result<T>(data);
        public static new Result<T> Failure(string errorMessage) => new Result<T>(errorMessage);
    }
}
