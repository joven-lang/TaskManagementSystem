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

        // Dashboard methods
        Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
        Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(string userId);

        // Sorting only (legacy - kept for backward compatibility)
        Task<IEnumerable<TaskEntity>> GetAllSortedAsync(string sortField, string sortOrder);

        // Complete method with filtering, sorting, and pagination
        Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetAllFilteredSortedPagedAsync(
            string sortField,
            string sortOrder,
            int page,
            int pageSize,
            string? statusFilter,
            string? priorityFilter,
            string? searchTerm);
    }
}