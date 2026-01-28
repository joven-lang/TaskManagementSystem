using TaskManagementSystem.Models;
using TaskManagementSystem.Repositories.Interfaces;
using TaskManagementSystem.Services.Interfaces;
using TaskManagementSystem.ViewModels;

namespace TaskManagementSystem.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly ITaskRepository _taskRepository;

        public DashboardService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var allTasks = await _taskRepository.GetAllTasksAsync();
            return CalculateDashboardMetrics(allTasks);
        }

        public async Task<DashboardViewModel> GetDashboardDataForUserAsync(string userId)
        {
            var userTasks = await _taskRepository.GetTasksByUserIdAsync(userId);
            return CalculateDashboardMetrics(userTasks);
        }

        private DashboardViewModel CalculateDashboardMetrics(IEnumerable<TaskEntity> tasks)
        {
            var today = DateTime.Today;
            var endOfWeek = today.AddDays(7);

            var viewModel = new DashboardViewModel
            {
                TotalTasks = tasks.Count(),
                PendingTasks = tasks.Count(t => t.Status == "Pending"),
                CompletedTasks = tasks.Count(t => t.Status == "Completed"),

                // Handle nullable DueDate
                OverdueTasks = tasks.Count(t =>
                    t.DueDate.HasValue &&
                    t.DueDate.Value < today &&
                    t.Status != "Completed"),

                TasksDueToday = tasks.Count(t =>
                    t.DueDate.HasValue &&
                    t.DueDate.Value.Date == today &&
                    t.Status != "Completed"),

                TasksDueThisWeek = tasks.Count(t =>
                    t.DueDate.HasValue &&
                    t.DueDate.Value >= today &&
                    t.DueDate.Value <= endOfWeek &&
                    t.Status != "Completed")
            };

            viewModel.CalculateCompletionRate();

            return viewModel;
        }
    }
}