using StudentManagement.Models;

namespace StudentManagement.Services
{
    public interface IUserService
    {
        List<User> Get();
        User Get(string id);
        User Create(User user);
        void Update(string id, User user);
        void Remove(string id);
        bool AuthenticateUser(string NIC, string password, string role);
        User RegisterUser(string name, string NIC, string password, string mobile, string email, string role);

        User GetUserDetails(string id);

        User GetById(string id);

        void UpdateStatus(string userId, string newStatus);
    }
}
