// ADDED - Full file
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class NotificationEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int TaskId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty; // "DueTomorrow", "DueToday", "Overdue"

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public TaskEntity? Task { get; set; }
    }
}