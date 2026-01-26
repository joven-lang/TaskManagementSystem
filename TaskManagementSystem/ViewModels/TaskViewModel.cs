using System;

namespace TaskManagementSystem.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string Status { get; set; } = string.Empty; // ✅ string
        public int Priority { get; set; }                   // ✅ int

        public DateTime? DueDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
