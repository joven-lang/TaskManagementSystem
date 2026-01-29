using TaskManagementSystem.ViewModels;

namespace TaskManagementSystem.Services.Interfaces
{
    public interface ITaskService
    {
        Task<(IEnumerable<TaskViewModel> Tasks, int TotalCount)> GetAllTasksAsync(
            string sortField = "CreatedAt",
            string sortOrder = "desc",
            int page = 1,
            int pageSize = 10,
            string? statusFilter = null,
            string? priorityFilter = null,
            string? searchTerm = null);

        Task<TaskViewModel?> GetTaskByIdAsync(int id);
        Task CreateTaskAsync(TaskCreateViewModel model, string? userId = null);
        Task<bool> UpdateTaskAsync(int id, TaskViewModel model);
        Task<bool> DeleteTaskAsync(int id);
    }
}