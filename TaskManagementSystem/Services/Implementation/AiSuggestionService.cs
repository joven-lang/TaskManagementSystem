using TaskManagementSystem.Repositories.Interfaces;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Services
{
    public class AiSuggestionService : IAiSuggestionService
    {
        private readonly ITaskRepository _taskRepository;

        public AiSuggestionService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public List<string> GenerateSuggestions(string userId)
        {
            var suggestions = new List<string>();

            var tasks = _taskRepository
                .GetAll()
                .Result
                .Where(t => t.UserId == userId)
                .ToList();

            // 🧠 RULE 1: Average completion time (ESTIMATED)
            var completedTasks = tasks
                .Where(t => t.Status == "Completed")
                .ToList();

            if (completedTasks.Any())
            {
                var avgDays = completedTasks.Average(t =>
                    (DateTime.Today - t.CreatedAt.Date).TotalDays
                );

                suggestions.Add(
                    $"🧠 You usually complete tasks in about {Math.Round(avgDays)} days"
                );
            }

            // ⚠️ RULE 2: Near overdue tasks (Due Tomorrow)
            var nearOverdue = tasks.Any(t =>
                t.Status != "Completed" &&
                t.DueDate.HasValue &&
                t.DueDate.Value.Date == DateTime.Today.AddDays(1));

            if (nearOverdue)
            {
                suggestions.Add("⚠️ Some tasks might be overdue tomorrow");
            }

            // 🔥 RULE 3: Too many pending tasks
            var pendingCount = tasks.Count(t => t.Status == "Pending");
            if (pendingCount >= 5)
            {
                suggestions.Add("🔥 You have many pending tasks. Consider completing some today.");
            }

            return suggestions;
        }
    }
}
