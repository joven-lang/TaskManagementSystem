using TaskManagementSystem.ViewModels;


namespace TaskManagementSystem.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskViewModel>> GetAllTasksAsync();
        Task<TaskViewModel?> GetTaskByIdAsync(int id);
        Task CreateTaskAsync(TaskCreateViewModel model, string? userId = null);
        Task<bool> UpdateTaskAsync(int id, TaskViewModel model);
        Task<bool> DeleteTaskAsync(int id);
    }
}