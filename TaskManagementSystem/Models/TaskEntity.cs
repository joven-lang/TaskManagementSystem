using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    public class TaskEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Title { get; set; }

        public string? Description { get; set; }  // Make nullable if optional

        [Required]
        public string? Status { get; set; }

        public int Priority { get; set; }  // Make nullable if optional

        public DateTime? DueDate { get; set; }  // Nullable DateTime

        public DateTime CreatedAt { get; set; }

        public string? CreatedByUserId { get; set; }  // For user relationship
        public DateTime UpdatedAt { get; set; }
        public string? UserId { get; set; }

        // Navigation property (optional)
        public ApplicationUser? User { get; set; }
    }
}