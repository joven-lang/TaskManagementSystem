using System;

namespace TaskManagementSystem.ViewModels
{
    public class TaskCreateViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string? Status { get; set; }   // ✅ string
        public int Priority { get; set; }     // ✅ int

        public DateTime? DueDate { get; set; }
    }
}
