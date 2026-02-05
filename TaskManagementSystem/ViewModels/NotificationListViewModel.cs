// ============================================
// FILE 3: NotificationListViewModel.cs
// Location: ViewModels/NotificationListViewModel.cs
// Action: CREATE NEW FILE (this is a new file, doesn't exist yet)
// ============================================

using TaskManagementSystem.Models;

namespace TaskManagementSystem.ViewModels
{
    /// <summary>
    /// View model for the notification list page with pagination and filtering
    /// </summary>
    public class NotificationListViewModel
    {
        // ========== DATA ==========

        /// <summary>
        /// The notifications to display on current page
        /// </summary>
        public List<NotificationEntity> Notifications { get; set; } = new();

        // ========== PAGINATION ==========

        /// <summary>
        /// Current page number (1, 2, 3...)
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Total number of notifications (all pages)
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Total number of pages (calculated)
        /// Example: 50 items / 10 per page = 5 pages
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        /// <summary>
        /// Can we go to previous page?
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// Can we go to next page?
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;

        // ========== FILTERING ==========

        /// <summary>
        /// Current type filter (Overdue, DueToday, DueTomorrow, or null for all)
        /// </summary>
        public string? TypeFilter { get; set; }

        /// <summary>
        /// Current read status filter (true=read, false=unread, null=all)
        /// </summary>
        public bool? IsReadFilter { get; set; }

        // ========== DROPDOWN OPTIONS ==========

        /// <summary>
        /// Options for the type filter dropdown
        /// </summary>
        public Dictionary<string, string> TypeOptions { get; set; } = new()
        {
            { "", "All Types" },
            { "Overdue", "Overdue" },
            { "DueToday", "Due Today" },
            { "DueTomorrow", "Due Tomorrow" }
        };

        /// <summary>
        /// Options for the read status filter dropdown
        /// </summary>
        public Dictionary<string, string> ReadStatusOptions { get; set; } = new()
        {
            { "", "All Notifications" },
            { "false", "Unread Only" },
            { "true", "Read Only" }
        };

        // ========== HELPER METHODS ==========

        /// <summary>
        /// Convert DateTime to relative time string
        /// Example: "2 hours ago", "Just now", "3 days ago"
        /// </summary>
        public string GetRelativeTime(DateTime createdAt)
        {
            var timeSpan = DateTime.Now - createdAt;

            if (timeSpan.TotalMinutes < 1)
                return "Just now";

            if (timeSpan.TotalMinutes < 60)
            {
                var minutes = (int)timeSpan.TotalMinutes;
                return $"{minutes} minute{(minutes > 1 ? "s" : "")} ago";
            }

            if (timeSpan.TotalHours < 24)
            {
                var hours = (int)timeSpan.TotalHours;
                return $"{hours} hour{(hours > 1 ? "s" : "")} ago";
            }

            if (timeSpan.TotalDays < 7)
            {
                var days = (int)timeSpan.TotalDays;
                return $"{days} day{(days > 1 ? "s" : "")} ago";
            }

            if (timeSpan.TotalDays < 30)
            {
                var weeks = (int)(timeSpan.TotalDays / 7);
                return $"{weeks} week{(weeks > 1 ? "s" : "")} ago";
            }

            // For older notifications, show the date
            return createdAt.ToString("MMM dd, yyyy");
        }

        /// <summary>
        /// Group notifications by date category
        /// Returns: "Today", "Yesterday", "This Week", or "Older"
        /// </summary>
        public string GetDateGroup(DateTime createdAt)
        {
            var today = DateTime.Today;
            var date = createdAt.Date;

            if (date == today)
                return "Today";

            if (date == today.AddDays(-1))
                return "Yesterday";

            if (date >= today.AddDays(-7))
                return "This Week";

            return "Older";
        }
    }
}