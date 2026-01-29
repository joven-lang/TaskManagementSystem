using System.Collections.Generic;

namespace TaskManagementSystem.ViewModels
{
    public class TaskListViewModel
    {
        public List<TaskViewModel> Tasks { get; set; } = new();

        // Sorting properties
        public string CurrentSortField { get; set; } = "CreatedAt";
        public string CurrentSortOrder { get; set; } = "desc";

        // Pagination properties
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        // Filtering properties
        public string? StatusFilter { get; set; }
        public string? PriorityFilter { get; set; }
        public string? SearchTerm { get; set; }

        // Available options for dropdowns
        public Dictionary<string, string> SortFields { get; set; } = new()
        {
            { "Title", "Title" },
            { "Status", "Status" },
            { "Priority", "Priority" },
            { "DueDate", "Due Date" },
            { "CreatedAt", "Created Date" }
        };

        public Dictionary<string, string> SortOrders { get; set; } = new()
        {
            { "asc", "Ascending" },
            { "desc", "Descending" }
        };

        public Dictionary<string, string> StatusOptions { get; set; } = new()
        {
            { "", "All Statuses" },
            { "Pending", "Pending" },
            { "InProgress", "In Progress" },
            { "Completed", "Completed" }
        };

        public Dictionary<string, string> PriorityOptions { get; set; } = new()
        {
            { "", "All Priorities" },
            { "1", "High" },
            { "2", "Medium" },
            { "3", "Low" }
        };

        // Helper methods
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}