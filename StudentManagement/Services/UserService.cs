using MongoDB.Driver;
using StudentManagement.Models;
using System.Data;

namespace StudentManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _user;

        public UserService(IUserStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public User Create(User user)
        {
            _user.InsertOne(user);
            return user;
        }

        public List<User> Get()
        {
            return _user.Find(user => true).ToList();
        }

        public User Get(string id)
        {
            return _user.Find(user => user.Id == id).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _user.DeleteOne(user => user.Id == id);
        }

        public void Update(string id, User user)
        {
            var existingUser = _user.Find(x => x.Id == id).FirstOrDefault();

            if (existingUser == null)
            {
                // Handle the case where the train with the provided ID doesn't exist
                throw new Exception("User not found.");
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Mobile = user.Mobile;

            _user.ReplaceOne(x => x.Id == id, existingUser);
        }

        public bool AuthenticateUser(string nic, string password, string role)
        {
            // Find a user in the MongoDB collection with the provided NIC
            var user = _user.Find(u => u.Nic == nic && u.Role == role).FirstOrDefault();

            if (user == null)
            {
                // User with the provided NIC does not exist
                return false;
            }

            // Check if the provided password matches the stored hashed password
            if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                // Password matches, authentication successful
                return true;
            }

            // Password does not match
            return false;
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false; // Password hash does not match
                    }
                }
            }
            return true; // Password hash matches
        }

        public User RegisterUser(string name, string nic, string password, string mobile, string email, string role)
        {
            try
            {
                // Check if the NIC is already taken (optional)
                var existingUser = _user.Find(u => u.Nic == nic).FirstOrDefault();
                if (existingUser != null)
                {
                    throw new Exception("NIC is already registered.");
                }

                // Generate a new salt for the user
                byte[] salt;
                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    salt = hmac.Key;
                }

                // Hash the password using the generated salt
                byte[] passwordHash = HashPassword(password, salt);

                // Create a new User object
                var newUser = new User
                {
                    Name = name,
                    Nic = nic,
                    Mobile = mobile,
                    Email = email,
                    Role = role,
                    PasswordHash = passwordHash,
                    PasswordSalt = salt,
                    Status = "Active",
                };

                // Insert the new user into the MongoDB collection
                _user.InsertOne(newUser);

                return newUser;
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., database errors
                // You can log the exception or perform additional error handling here
                throw new Exception(ex.ToString());
            }
        }


        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
                return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public User GetUserDetails(string nic)
        {
            return _user.Find(u => u.Nic == nic).FirstOrDefault();
        }

        public User GetById(string userId)
        {
            return _user.Find(user => user.Id == userId).FirstOrDefault();
        }

        public void UpdateStatus(string userId, string newStatus)
        {
            var existingUser = _user.Find(x => x.Id == userId).FirstOrDefault();

            if (existingUser == null)
            {
                // Handle the case where the train with the provided ID doesn't exist
                throw new Exception("User not found.");
            }

            existingUser.Status = newStatus;

            _user.ReplaceOne(x => x.Id == userId, existingUser);
        }


    }
}
