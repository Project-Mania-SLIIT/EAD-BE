using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using StudentManagement.Models;
using System.Data;
using System.Reflection;
using System.Xml.Linq;

namespace StudentManagement.Services
{
    public class TrainService : ITrainService
    {
        private readonly IMongoCollection<Train> _train;

        public TrainService(ITrainStoreDatabaseSetting settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _train = database.GetCollection<Train>(settings.TrainCollectionName);
        }

        public Train Create(Train train)
        {
            // Create a new User object
            var newTrain = new Train
            {
                TrainName = train.TrainName,
                StartingPlace = train.StartingPlace,
                Destination = train.Destination,
                NumberOfSeats = train.NumberOfSeats,
                Price = train.Price,
                Time = train.Time,
                TrainNo = train.TrainNo,
                Status = "Active",
            };

            _train.InsertOne(newTrain);
            return train;
        }

        public List<Train> Get()
        {
            return _train.Find(train => true).ToList();
        }

        public Train GetById(string trainId)
        {
            return _train.Find(train => train.Id == trainId).FirstOrDefault();
        }

        public void UpdateStatus(string trainId, string newStatus)
        {
            var existingTrain = _train.Find(x => x.Id == trainId).FirstOrDefault();

            if (existingTrain == null)
            {
                // Handle the case where the train with the provided ID doesn't exist
                throw new Exception("Train not found.");
            }

            existingTrain.Status = newStatus;

            _train.ReplaceOne(x => x.Id == trainId, existingTrain);
        }


    }
}
