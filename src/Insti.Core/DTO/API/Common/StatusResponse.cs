namespace Insti.Core.DTO.API.Common
{
    public class StatusResponse
    {
        public StatusResponse(string status, string message)
        {
            Status = status;
            Message = message;
        }

        public string? Status { get; set; }

        public string? Message { get; set; }
    }
}
