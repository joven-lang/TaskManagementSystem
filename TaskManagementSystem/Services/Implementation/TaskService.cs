using TaskManagementSystem.Models;
using TaskManagementSystem.Repositories.Interfaces;
using TaskManagementSystem.Services.Interfaces;
using TaskManagementSystem.ViewModels;

namespace TaskManagementSystem.Services.Implementation
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<(IEnumerable<TaskViewModel> Tasks, int TotalCount)> GetAllTasksAsync(
            string sortField = "CreatedAt",
            string sortOrder = "desc",
            int page = 1,
            int pageSize = 10,
            string? statusFilter = null,
            string? priorityFilter = null,
            string? searchTerm = null)
        {
            var result = await _taskRepository.GetAllFilteredSortedPagedAsync(
                sortField,
                sortOrder,
                page,
                pageSize,
                statusFilter,
                priorityFilter,
                searchTerm
            );

            var taskViewModels = result.Tasks.Select(t => new TaskViewModel
            {
                Id = t.Id,
                Title = t.Title ?? string.Empty,
                Description = t.Description,
                Status = t.Status ?? string.Empty,
                Priority = t.Priority,
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });

            return (taskViewModels, result.TotalCount);
        }

        public async Task<TaskViewModel?> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetById(id);
            if (task == null)
                return null;

            return new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title ?? string.Empty,
                Description = task.Description,
                Status = task.Status ?? string.Empty,
                Priority = task.Priority,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }

        public async Task CreateTaskAsync(TaskCreateViewModel model, string? userId = null)
        {
            var task = new TaskEntity
            {
                Title = model.Title,
                Description = model.Description,
                Status = model.Status,
                Priority = model.Priority,
                DueDate = model.DueDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = userId,
                CreatedByUserId = userId
            };

            await _taskRepository.Create(task);
            await _taskRepository.Save();
        }

        public async Task<bool> UpdateTaskAsync(int id, TaskViewModel model)
        {
            var task = await _taskRepository.GetById(id);
            if (task == null)
                return false;

            task.Title = model.Title;
            task.Description = model.Description;
            task.Status = model.Status;
            task.Priority = model.Priority;
            task.DueDate = model.DueDate;
            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepository.Update(task);
            await _taskRepository.Save();
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetById(id);
            if (task == null)
                return false;

            await _taskRepository.Delete(id);
            await _taskRepository.Save();
            return true;
        }
    }
}