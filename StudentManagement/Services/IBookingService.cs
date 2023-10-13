using StudentManagement.Models;

namespace StudentManagement.Services
{
    public interface IBookingService
    {
        Booking Create(Booking booking);
        List<Booking> Get(string userId);
        Booking Update(Booking booking);
        bool Delete(string bookingId);

        bool HasBookingsForDateOrFuture(string trainId, DateTime date);

    }
}
