namespace Insti.Core.Constants
{
    public static class ErrorCodes
    {
        private const string BadRequestTemplate = "Bad request - Error code: {1}";
        
        public enum Code
        {
            InvalidUserId
        }

        public static string BadRequest(Code code) => string.Format(BadRequestTemplate, code);
    }
}
