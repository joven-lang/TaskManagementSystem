using System.Collections.Generic;

namespace TaskManagementSystem.ViewModels
{
    public class TaskListViewModel
    {
        public List<TaskViewModel> Tasks { get; set; } = new();

        // Sorting
        public string CurrentSortField { get; set; } = "CreatedAt";
        public string CurrentSortOrder { get; set; } = "desc";

        public Dictionary<string, string> SortFields { get; set; } = new()
        {
            { "Title", "Title" },
            { "Status", "Status" },
            { "Priority", "Priority" },
            { "DueDate", "Due Date" },
            { "CreatedAt", "Created Date" },
            { "CreatedBy", "Created By" } // ADDED
        };

        public Dictionary<string, string> SortOrders { get; set; } = new()
        {
            { "asc", "Ascending" },
            { "desc", "Descending" }
        };

        // Filtering
        public string? StatusFilter { get; set; }
        public string? PriorityFilter { get; set; }
        public string? SearchTerm { get; set; }

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

        // Pagination
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}