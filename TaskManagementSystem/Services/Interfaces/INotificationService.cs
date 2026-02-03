// ADDED - Full file
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(string userId, int taskId, string message, string type);
        Task<List<NotificationEntity>> GetUserNotificationsAsync(string userId);
        Task<int> GetUnreadCountAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(string userId);
        Task CheckAndCreateDueDateNotificationsAsync();
    }
}