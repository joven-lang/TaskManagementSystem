using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementSystem.Services.Interfaces;
using TaskManagementSystem.ViewModels;


namespace TaskManagementSystem.Controllers
{
    [Authorize] // Require authentication for all actions
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }







        // GET: Task
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            var viewModel = new TaskListViewModel
            {
                Tasks = tasks.ToList()
            };
            return View(viewModel);
        }









        // GET: Task/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            { 
                return NotFound();
            }
            return View(task);
        }








        // GET: Task/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {       
                return View(model);
            }

            // Get current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _taskService.CreateTaskAsync(model, userId);
            TempData["SuccessMessage"] = "Task created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Task/Edit/5
        [Authorize(Roles = "Admin")] // Only Admin can edit
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Task/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only Admin can edit
        public async Task<IActionResult> Edit(int id, TaskViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _taskService.UpdateTaskAsync(id, model);
            if (!result)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Task updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Task/Delete/5
        [Authorize(Roles = "Admin")] // Only Admin can delete
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only Admin can delete
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Task deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}