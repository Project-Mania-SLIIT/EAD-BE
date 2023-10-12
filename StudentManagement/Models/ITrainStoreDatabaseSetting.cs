namespace StudentManagement.Models
{
    public interface ITrainStoreDatabaseSetting
    {
        string TrainCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
