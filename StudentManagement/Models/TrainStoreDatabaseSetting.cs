namespace StudentManagement.Models
{
    public class TrainStoreDatabaseSetting : ITrainStoreDatabaseSetting
    {
        public string TrainCollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}
