using TaskManagementSystem.Models;

namespace TaskManagementSystem.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalTasks { get; set; }
        public int PendingTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int OverdueTasks { get; set; }

        public decimal CompletionRate { get; set; }
        public int TasksDueToday { get; set; }
        public int TasksDueThisWeek { get; set; }

        // ✅ IMPORTANT: para sa Recent Tasks table sa Dashboard
        public List<TaskEntity> RecentTasks { get; set; } = new();

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
