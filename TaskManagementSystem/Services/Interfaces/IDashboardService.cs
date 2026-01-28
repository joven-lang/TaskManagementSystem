using TaskManagementSystem.ViewModels;

namespace TaskManagementSystem.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
        Task<DashboardViewModel> GetDashboardDataForUserAsync(string userId);
    }
}