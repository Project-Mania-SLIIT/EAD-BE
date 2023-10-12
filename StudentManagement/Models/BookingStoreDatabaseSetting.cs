namespace StudentManagement.Models
{
    public class BookingStoreDatabaseSetting : IBookingStoreDatabaseSetting
    {
        public string BookingCollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}
