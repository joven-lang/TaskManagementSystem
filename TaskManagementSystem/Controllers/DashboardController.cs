using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementSystem.Services;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IDashboardService dashboardService,
            ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);///ID ng taong naka-login ngayon    

                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var dashboardData = await _dashboardService.GetDashboardDataForUserAsync(userId);///dashboard data ng naka-login na user

                return View(dashboardData);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error loading dashboard");
                TempData["Error"] = "An error occurred while loading the dashboard.";///nire-record ang error, nagpapakita ng error message,
                return View(new ViewModels.DashboardViewModel());
            }
        }

        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> AdminDashboard()///method na nagpapakita ng Admin Dashboard
        {
            try
            {
                var dashboardData = await _dashboardService.GetDashboardDataAsync();///lahat ng data para sa Admin Dashboard.
                return View("Index", dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading admin dashboard");
                TempData["Error"] = "An error occurred while loading the dashboard.";
                return View("Index", new ViewModels.DashboardViewModel());///nire-record ang problema, nagpapakita ng error message, at ibinabalik ang dashboard page na walang laman
            }
        }
    }
}

