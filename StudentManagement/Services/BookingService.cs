using MongoDB.Driver;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMongoCollection<Booking> _booking;
        private readonly ITrainService _trainService; // Inject the TrainService

        public BookingService(IBookingStoreDatabaseSetting settings, IMongoClient mongoClient, ITrainService trainService)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _booking = database.GetCollection<Booking>(settings.BookingCollectionName);
            _trainService = trainService; // Initialize the TrainService dependency
        }

        public Booking Create(Booking booking)
        {
            // Check if the total booked seats exceed the available seats on the train
            int totalBookedSeats = GetTotalBookedSeats(booking.TrainId, booking.Date.ToString().Substring(0, 10));

            // Retrieve the train information based on the TrainId
            Train train = _trainService.GetById(booking.TrainId); // Use _trainService

            if (totalBookedSeats + booking.NumberOfSeats > train.NumberOfSeats)
            {
                // Total booked seats exceed the available seats on the train, throw an error
                throw new Exception("All seats are already booked for this train on the specified date.");
            }

            // Create a new User object
            var newBooking = new Booking
            {
                UserId = booking.UserId,
                TrainId = booking.TrainId,
                Date = booking.Date.ToString().Substring(0, 10),
                NumberOfSeats = booking.NumberOfSeats,
                Price = booking.Price,
            };

            _booking.InsertOne(newBooking);
            return newBooking;
        }

        public List<Booking> Get(string userId)
        {
            var filter = Builders<Booking>.Filter.Eq("UserId", userId);
            var bookings = _booking.Find(filter).ToList();

            // Populate train data for each booking
            foreach (var booking in bookings)
            {
                booking.Train = _trainService.GetById(booking.TrainId);
            }

            return bookings;
        }

        private int GetTotalBookedSeats(string trainId, string date)
        {
            // Find all booking records for the specified train on the specified date
            var filter = Builders<Booking>.Filter.Eq(x => x.TrainId, trainId) & Builders<Booking>.Filter.Eq(x => x.Date, date);
            var bookedSeatsList = _booking.Find(filter).ToList();

            // Calculate the total booked seats
            int totalBookedSeats = bookedSeatsList.Sum(x => x.NumberOfSeats);

            return totalBookedSeats;
        }

        public Booking Update(Booking booking)
        {
            // Retrieve the existing booking
            var existingBooking = _booking.Find(x => x.Id == booking.Id).FirstOrDefault();
            if (existingBooking == null)
            {
                throw new Exception("Booking not found.");
            }

            // Parse the existing booking date from string to DateTime
            DateTime existingBookingDate = DateTime.Parse(existingBooking.Date);

            // Calculate the date difference between the current date and the booking date
            TimeSpan dateDifference = existingBookingDate - DateTime.Now;

            // Check if the current date is more than two days before the booking date
            if (dateDifference.TotalDays < 2)
            {
                throw new Exception("Booking can only be updated if the current date is more than two days before the booking date.");
            }

            // Check if the total booked seats exceed the available seats on the train
            int totalBookedSeats = GetTotalBookedSeats(booking.TrainId, booking.Date.ToString().Substring(0, 10));
            Train train = _trainService.GetById(booking.TrainId);

            if (totalBookedSeats - existingBooking.NumberOfSeats + booking.NumberOfSeats > train.NumberOfSeats)
            {
                throw new Exception("Updating this booking would exceed the available seats on the train on the specified date.");
            }

            // Update the booking details
            existingBooking.UserId = booking.UserId;
            existingBooking.TrainId = booking.TrainId;
            existingBooking.Date = booking.Date.ToString().Substring(0, 10);
            existingBooking.NumberOfSeats = booking.NumberOfSeats;
            existingBooking.Price = booking.Price;

            // Replace the existing booking with the updated booking
            _booking.ReplaceOne(x => x.Id == booking.Id, existingBooking);

            return existingBooking;
        }

        public bool Delete(string bookingId)
        {
            // Retrieve the existing booking
            var existingBooking = _booking.Find(x => x.Id == bookingId).FirstOrDefault();
            if (existingBooking == null)
            {
                throw new Exception("Booking not found.");
            }

            // Parse the existing booking date from string to DateTime
            DateTime existingBookingDate = DateTime.Parse(existingBooking.Date);

            // Calculate the date difference between the current date and the booking date
            TimeSpan dateDifference = existingBookingDate - DateTime.Now;

            // Check if the current date is more than two days before the booking date
            if (dateDifference.TotalDays < 2)
            {
                throw new Exception("Booking can only be deleted if the current date is more than two days before the booking date.");
            }

            _booking.DeleteOne(booking => booking.Id == bookingId);

            return true;


        }

        public bool HasBookingsForDateOrFuture(string trainId, DateTime date)
        {
            // Define a filter to find bookings for the specified train and date (today or future)
            var filter = Builders<Booking>.Filter.Eq(x => x.TrainId, trainId) & Builders<Booking>.Filter.Gte(x => x.Date, date.ToString("yyyy-MM-dd"));

            // Check if there are any bookings matching the filter
            return _booking.Find(filter).Any();
        }

    }
}
