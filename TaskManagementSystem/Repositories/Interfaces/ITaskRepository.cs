
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        TaskEntity? GetById(int id);
        IEnumerable<TaskEntity> GetAll();
        void Create(TaskEntity task);
        void Update(TaskEntity task);
        void Delete(int id);
        void Save();
    }
}
