using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace StudentManagement.Models
{
    //create user model
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = String.Empty;

        [BsonElement("nic")]
        public string Nic { get; set; } = String.Empty;

        [BsonElement("mobile")]
        public string Mobile { get; set; } = String.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = String.Empty;

        [BsonElement("role")]
        public string Role { get; set; } = String.Empty;

        [BsonElement("passwordHash")]
        public byte[]? PasswordHash { get; set; }

        [BsonElement("passwordSalt")]
        public byte[]? PasswordSalt { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Active";
    }
}
