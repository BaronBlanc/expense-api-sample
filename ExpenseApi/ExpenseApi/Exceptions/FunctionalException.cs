using Microsoft.AspNetCore.Http;

namespace ExpenseApi.Exceptions
{
    public class FunctionalException : Exception
    {
        private readonly int StatusCode;

        public FunctionalException(string message) : base(message)
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }

        public FunctionalException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
