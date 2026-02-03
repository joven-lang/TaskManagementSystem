// ADDED - Full file
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationAsync(string userId, int taskId, string message, string type)
        {
            // Check if notification already exists for this task and type
            var existingNotification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.UserId == userId && n.TaskId == taskId && n.Type == type && !n.IsRead);

            if (existingNotification == null)
            {
                var notification = new NotificationEntity
                {
                    UserId = userId,
                    TaskId = taskId,
                    Message = message,
                    Type = type,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<NotificationEntity>> GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Include(n => n.Task)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task CheckAndCreateDueDateNotificationsAsync()
        {
            var now = DateTime.Now;
            var tomorrow = now.AddDays(1).Date;
            var today = now.Date;

            // Get all tasks that are not completed
            var tasks = await _context.Tasks
                .Where(t => t.Status != "Completed" && t.DueDate.HasValue)
                .ToListAsync();

            foreach (var task in tasks)
            {
                if (task.DueDate.HasValue && !string.IsNullOrEmpty(task.UserId))
                {
                    var dueDate = task.DueDate.Value.Date;

                    // Check for overdue tasks
                    if (dueDate < today)
                    {
                        await CreateNotificationAsync(
                            task.UserId,
                            task.Id,
                            $"Task '{task.Title}' is overdue!",
                            "Overdue"
                        );
                    }
                    // Check for tasks due today
                    else if (dueDate == today)
                    {
                        await CreateNotificationAsync(
                            task.UserId,
                            task.Id,
                            $"Task '{task.Title}' is due today!",
                            "DueToday"
                        );
                    }
                    // Check for tasks due tomorrow
                    else if (dueDate == tomorrow)
                    {
                        await CreateNotificationAsync(
                            task.UserId,
                            task.Id,
                            $"Task '{task.Title}' is due tomorrow!",
                            "DueTomorrow"
                        );
                    }
                }
            }
        }
    }
}