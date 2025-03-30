namespace BTM.Account.Domain.Abstractions
{

    public class Result
    {
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; } // List to hold multiple error messages
        public object? Data { get; set; }

        public static Result SuccessResult(object? data = null)
        {
            return new Result { IsSuccess = true, ErrorMessages = new List<string>(), Data = data };
        }

        public static Result FailureResult(List<string> errorMessages)
        {
            return new Result { IsSuccess = false, ErrorMessages = errorMessages };
        }

        public static Result FailureResult(string errorMessage)
        {
            return new Result { IsSuccess = false, ErrorMessages = new List<string> { errorMessage } };
        }
    }

    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessages { get; set; }
        public T? Data { get; set; }
        public Result<T> SuccessResult(T data)
        {
            return new Result<T> { IsSuccess = true, ErrorMessages = string.Empty, Data = data };
        }
        public Result<T> FailureResult(string message)
        {
            return new Result<T> { IsSuccess = false, ErrorMessages = message };
        }
    }
}
