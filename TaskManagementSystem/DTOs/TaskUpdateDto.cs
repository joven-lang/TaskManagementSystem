using System;

namespace TaskManagementSystem.DTOs
{
    public class TaskUpdateDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string Status { get; set; } = "Pending";  // ✅ string
        public int Priority { get; set; }                // ✅ int

        public DateTime? DueDate { get; set; }
    }
}
