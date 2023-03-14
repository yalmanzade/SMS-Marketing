namespace SMS_Marketing.Models
{
    //This model is not being used. It is just for testing.
    public class UserPermissions
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string OrganizationId { get; set; } = string.Empty;
        public bool IsPost { get; set; } = false;
        public bool IsUserManagement { get; set; } = false;
        public bool IsInsight { get; set; } = false;
        public bool IsCustomerManagment { get; set; } = false;
        public bool IsSystemManager { get; set; } = false;
    }
}
