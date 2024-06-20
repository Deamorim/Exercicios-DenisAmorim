namespace Questao5.Application.Utils
{
    public class ErrorResponse
    {
        public string ErrorMessage { get; set; }
        public string ErrorType { get; set; }

        public ErrorResponse(string errorMessage, string errorType)
        {
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }
    }

}
