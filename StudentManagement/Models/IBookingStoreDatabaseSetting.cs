namespace StudentManagement.Models
{
    public interface IBookingStoreDatabaseSetting
    {
        string BookingCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
