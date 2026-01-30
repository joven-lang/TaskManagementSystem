using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()///method na nagpapakita ng Home page
        {
            return View();
        }

        public IActionResult Privacy()///method na nagpapakita ng Privacy page
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]///huwag i-save o i-cache ang page,
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });///method na nagpapakita ng Error page
        }
    }
}
