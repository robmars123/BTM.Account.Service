namespace BTM.Account.MVC.Client.Models
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static Result SuccessResult(object data = null)
        {
            return new Result { Success = true, Message = "Operation was successful.", Data = data };
        }

        public static Result FailureResult(string message)
        {
            return new Result { Success = false, Message = message };
        }
    }
}
