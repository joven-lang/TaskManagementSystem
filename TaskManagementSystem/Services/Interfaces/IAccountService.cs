using TaskManagementSystem.ViewModels;


namespace TaskManagementSystem.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<bool> RegisterAsync(RegisterViewModel model, string role = "User");
        Task<bool> IsUserInRoleAsync(string userId, string role);
    }
}