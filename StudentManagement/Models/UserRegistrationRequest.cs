namespace StudentManagement.Models
{
    public class UserRegistrationRequest
    {
        public string Name { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Email {  get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
