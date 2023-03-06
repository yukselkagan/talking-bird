namespace IdentityService.Models
{
    public class UserToken
    {
        public User User { get; set; }
        public string AccessToken { get; set; }
    }
}
