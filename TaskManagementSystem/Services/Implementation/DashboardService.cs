using TaskManagementSystem.Models;
using TaskManagementSystem.Repositories.Interfaces;
using TaskManagementSystem.Services.Interfaces;
using TaskManagementSystem.ViewModels;

namespace TaskManagementSystem.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IAiSuggestionService _aiSuggestionService;

        // ✅ UPDATED CONSTRUCTOR
        public DashboardService(
            ITaskRepository taskRepository,
            IAiSuggestionService aiSuggestionService)
        {
            _taskRepository = taskRepository;
            _aiSuggestionService = aiSuggestionService;
        }

        // ============================
        // ADMIN DASHBOARD
        // ============================
        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var allTasks = await _taskRepository.GetAllTasksAsync();
            var model = CalculateDashboardMetrics(allTasks);

            // 🤖 AI Suggestions (Admin – optional userId)
            model.AiSuggestions = _aiSuggestionService.GenerateSuggestions(null);

            return model;
        }

        // ============================
        // USER DASHBOARD
        // ============================
        public async Task<DashboardViewModel> GetDashboardDataForUserAsync(string userId)
        {
            var userTasks = await _taskRepository.GetTasksByUserIdAsync(userId);
            var model = CalculateDashboardMetrics(userTasks);

            // 🤖 AI Suggestions
            model.AiSuggestions = _aiSuggestionService.GenerateSuggestions(userId);

            return model;
        }

        // ============================
        // DASHBOARD COMPUTATION
        // ============================
        private DashboardViewModel CalculateDashboardMetrics(IEnumerable<TaskEntity> tasks)
        {
            var today = DateTime.Today;
            var endOfWeek = today.AddDays(7);

            var viewModel = new DashboardViewModel
            {
                TotalTasks = tasks.Count(),

                PendingTasks = tasks.Count(t =>
                    t.Status == "Pending"),

                CompletedTasks = tasks.Count(t =>
                    t.Status == "Completed"),

                OverdueTasks = tasks.Count(t =>
                    t.DueDate.HasValue &&
                    t.DueDate.Value.Date < today &&
                    t.Status != "Completed"),

                TasksDueToday = tasks.Count(t =>
                    t.DueDate.HasValue &&
                    t.DueDate.Value.Date == today &&
                    t.Status != "Completed"),

                TasksDueThisWeek = tasks.Count(t =>
                    t.DueDate.HasValue &&
                    t.DueDate.Value.Date >= today &&
                    t.DueDate.Value.Date <= endOfWeek &&
                    t.Status != "Completed"),

                // ✅ IMPORTANT: Recent Tasks (latest 5)
                RecentTasks = tasks
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(5)
                    .ToList()
            };

            viewModel.CalculateCompletionRate();

            return viewModel;
        }
    }
}
