namespace Questao5.Application.Utils
{
    public class Result<T>
    {
        public bool Success { get; private set; }
        public T Data { get; private set; }
        public string ErrorMessage { get; private set; }
        public string ErrorCode { get; private set; } 

        private Result(bool success, T data, string errorMessage, string errorCode)
        {
            Success = success;
            Data = data;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        private Result() { }

        public static Result<T> Successfully(T data)
        {
            return new Result<T> { Success = true, Data = data };
        }

        public static Result<T> Fail(string errorMessage, string errorCode = "")
        {
            return new Result<T> { Success = false, ErrorMessage = errorMessage, ErrorCode = errorCode };
        }
    }
}
