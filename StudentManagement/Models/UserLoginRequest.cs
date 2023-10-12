namespace StudentManagement.Models
{
    public class UserLoginRequest
    {
        public string NIC { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Role {  get; set; } = string.Empty;
    }
}
