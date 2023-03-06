namespace IdentityService.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;        
        public string PasswordHash { get; set; } = string.Empty;
        public byte[] PasswordSalt { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = "";
        public string ProfileImage { get; set; } = string.Empty;
    }
}
