namespace BTM.Account.MVC.Client.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public List<string?> ErrorMessages { get; set; }
        public object? Data { get; set; }
    }
}
