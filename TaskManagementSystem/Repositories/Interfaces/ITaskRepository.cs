using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        // Existing methods
        Task<IEnumerable<TaskEntity>> GetAll();
        Task<TaskEntity?> GetById(int id);
        Task Create(TaskEntity task);
        Task Update(TaskEntity task);
        Task Delete(int id);
        Task Save();

        // New methods for Dashboard
        Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
        Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(string userId);
    }
}