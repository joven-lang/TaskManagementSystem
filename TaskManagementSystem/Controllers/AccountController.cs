using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Services.Interfaces;
using TaskManagementSystem.ViewModels;


namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)                                    ///method para ipakita ang Login page
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)

        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)                                                         ///page na babalikan pagkatapos mag-login
            {
                return View(model);
            }

            var result = await _accountService.LoginAsync(model);                            ///i-check kung tama ang username at password             

            if (result)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Task");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }






        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()                         ///Ipinapakita nito ang Register page (UI).
        {
            return View();
        }




        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)///tumatanggap ng impormasyon sa pag-register
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.RegisterAsync(model, "User");


            if (result)
            {
                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();                          ///method na nagla-logout ng user
            return RedirectToAction("Login");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()                        ///method na nagpapakita ng Access Denied page
        {
            return View();
        }
    }
}