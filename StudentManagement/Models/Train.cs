using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace StudentManagement.Models
{
    //create train model
    public class Train
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("trainName")]
        public string TrainName { get; set; } = String.Empty;

        [BsonElement("startingPlace")]
        public string StartingPlace { get; set; } = String.Empty;

        [BsonElement("destination")]
        public string Destination { get; set; } = String.Empty;

        [BsonElement("numberOfSeats")]
        public int NumberOfSeats { get; set; } = 0;

        [BsonElement("price")]
        public int Price { get; set; } = 0;

        [BsonElement("time")]
        public string Time { get; set; } = String.Empty;

        [BsonElement("trainNo")]
        public int TrainNo { get; set; } = 0;

        [BsonElement("status")]
        public string Status { get; set; } = String.Empty;

    }
}
