using TaskManagementSystem.Models;

namespace TaskManagementSystem.ViewModels
{
    public class DashboardViewModel
    {
        // ===== COUNTS =====
        public int TotalTasks { get; set; }
        public int PendingTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int OverdueTasks { get; set; }

        // ===== PROGRESS / INSIGHTS =====
        public decimal CompletionRate { get; set; }
        public int TasksDueToday { get; set; }
        public int TasksDueThisWeek { get; set; }

        // ===== RECENT TASKS (Dashboard Table) =====
        public List<TaskEntity> RecentTasks { get; set; } = new();

        // ===== 🤖 AI-STYLE SUGGESTIONS =====
        public List<string> AiSuggestions { get; set; } = new();

        // ===== CALCULATIONS =====
        public void CalculateCompletionRate()
        {
            if (TotalTasks > 0)
            {
                CompletionRate = Math.Round(
                    (decimal)CompletedTasks / TotalTasks * 100, 1
                );
            }
            else
            {
                CompletionRate = 0;
            }
        }
    }
}
