namespace FreeExam.Application.Contracts
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public int? StatusCode { get; set; }
        public string Message { get; set; }
        public Result(bool isSuccess,string message) {
            IsSuccess = isSuccess;
            Message = message;
        }
        public Result(bool isSuccess, int statusCode, string message) : this(isSuccess, message)
        {
            StatusCode = statusCode;
        }
        public static Result Success() => new Result(true, null);
        public static Result Failure(string error, int statusCode = 400) 
                    => new Result(false, statusCode, error);
    }
    public class Result<T> : Result
    {
        public T? Data { get; set; }
        public Result( T data) : base(true,null)
        {
            Data = data;
        }
        public Result( int stutosCode, string message) : base(false, stutosCode, message)
        {
        }
        public static Result<T> Success(T data) => new Result<T>(data);
        
        public static Result<T> Failure(string message , int stutosCode = 400) => new Result<T>(stutosCode, message);
        
    }
}
