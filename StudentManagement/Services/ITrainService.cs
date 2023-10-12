using StudentManagement.Models;

namespace StudentManagement.Services
{
    public interface ITrainService
    {
        Train Create(Train train);
        List<Train> Get();

        Train GetById(string id);

        void UpdateStatus(string trainId, string newStatus);
    }
}
