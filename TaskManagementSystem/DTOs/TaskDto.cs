using System;

namespace TaskManagementSystem.DTOs///lahat ng impormasyon tungkol sa isang task.
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public int OrderByNumber { get; set; }
        public DateTime? DueDate { get; set; }
    }
}