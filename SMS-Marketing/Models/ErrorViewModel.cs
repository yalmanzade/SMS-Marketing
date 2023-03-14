namespace SMS_Marketing.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class UserErrorModel
    {
        public string ErrorMessage { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public int ErrorCode { get; set; } = 100;
    }
}