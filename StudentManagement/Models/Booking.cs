using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace StudentManagement.Models
{
    //create booking model
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("UserId")]
        public string UserId { get; set; } = String.Empty;

        [BsonElement("TrainId")]
        public string TrainId { get; set; } = String.Empty;

        [BsonElement("Date")]
        public string Date { get; set; } = String.Empty;

        [BsonElement("NumberOfSeats")]
        public int NumberOfSeats { get; set; } = 0;

        [BsonElement("Price")]
        public int Price { get; set; } = 0;

        // Add a property to store train information
        [BsonIgnore] // Ignore this property for MongoDB serialization
        public Train Train { get; set; }
    }
}
