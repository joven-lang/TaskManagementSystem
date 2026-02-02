using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementSystem.Services.Interfaces;
using TaskManagementSystem.ViewModels;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(ITaskService taskService, ILogger<CalendarController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        // GET: Calendar
        public IActionResult Index()                     ///binubuksan nito ang page at wala pang ginagawang ibang logic.
        {
            return View();
        }

        // GET: Calendar/GetEvents (AJAX)
        [HttpGet]
        // POST: Calendar/QuickCreate (AJAX)
        [HttpPost]
        public async Task<IActionResult> QuickCreate([FromBody] QuickCreateTaskRequest request)///function na tumatanggap ng data at gumagawa ng task.
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);                                   ///kunin ang value ng impormasyon ng naka-login na user.
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var taskViewModel = new TaskCreateViewModel
                {
                    Title = request.Title,
                    Description = request.Description ?? "",///lagayan ng data task
                    Status = "Pending",
                    Priority = request.Priority,
                    DueDate = request.DueDate
                };

                await _taskService.CreateTaskAsync(taskViewModel, userId);///pag nakalog in kana inutusan ka na netong mag save para dun sa naka log in na user
                   
                return Json(new
                {
                    success = true,
                    message = "Task created successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task from calendar");
                return StatusCode(500, new { success = false, error = "Failed to create task" });
            }
        }

        public class QuickCreateTaskRequest///ipinapadala galing UI papunta sa server
        {
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            public int Priority { get; set; } = 2;
            public DateTime DueDate { get; set; }
        }







        public async Task<IActionResult> GetEvents(DateTime? start, DateTime? end)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                // Get all tasks (you can add user filtering if needed)
                var result = await _taskService.GetAllTasksAsync(
                    sortField: "DueDate",
                    sortOrder: "asc",
                    page: 1,
                    pageSize: 1000 // Get all tasks for calendar
                );

                var events = result.Tasks
                    .Where(t => t.DueDate.HasValue)
                    .Select(t => new CalendarEventViewModel///Pinipili lang nito ang mga task na may deadline at ginagawang calendar events.
                    {
                        Id = t.Id,///Nilalagyan ng event details
                        Title = t.Title,
                        Start = t.DueDate!.Value,
                        End = t.DueDate!.Value,
                        AllDay = true,
                        BackgroundColor = GetColorByPriority(t.Priority),
                        BorderColor = GetColorByPriority(t.Priority),
                        Status = t.Status,
                        Priority = t.Priority,
                        Description = t.Description,
                        Url = Url.Action("Details", "Task", new { id = t.Id }) ?? string.Empty,
                        ExtendedProps = new Dictionary<string, object>
                        {
                            { "status", t.Status },
                            { "priority", t.Priority },//Link pappunta sa task
                            { "description", t.Description ?? "" }
                        }
                    })
                    .ToList();

                return Json(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching calendar events");
                return StatusCode(500, new { error = "Failed to load calendar events" });
            }
        }

        // POST: Calendar/UpdateEventDate (AJAX - Drag and Drop)
        [HttpPost]
        public async Task<IActionResult> UpdateEventDate([FromBody] UpdateEventDateRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var task = await _taskService.GetTaskByIdAsync(request.Id);
                if (task == null)
                {
                    return NotFound(new { success = false, message = "Task not found" });
                }

                // Update the due date
                task.DueDate = request.NewDate;

                var result = await _taskService.UpdateTaskAsync(request.Id, task);
                if (!result)
                {
                    return BadRequest(new { success = false, message = "Failed to update task" });
                }

                return Json(new
                {
                    success = true,
                    message = "Task date updated successfully",
                    newDate = request.NewDate
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating event date");
                return StatusCode(500, new { success = false, error = "Failed to update date" });
            }
        }

        // GET: Calendar/GetTaskDetails (AJAX - For popup)
        [HttpGet]
        public async Task<IActionResult> GetTaskDetails(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }

                return Json(new
                {
                    id = task.Id,
                    title = task.Title,
                    description = task.Description,
                    status = task.Status,
                    priority = task.Priority,
                    dueDate = task.DueDate?.ToString("yyyy-MM-dd"),
                    createdAt = task.CreatedAt.ToString("MMM dd, yyyy"),
                    detailsUrl = Url.Action("Details", "Task", new { id = task.Id }),
                    editUrl = Url.Action("Edit", "Task", new { id = task.Id })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching task details");
                return StatusCode(500);
            }
        }

        // Helper method to get color based on priority
        private string GetColorByPriority(int priority)
        {
            return priority switch
            {
                1 => "#dc3545", // High - Red
                2 => "#ffc107", // Medium - Yellow
                3 => "#6c757d", // Low - Gray
                _ => "#0d6efd"  // Default - Blue
            };
        }
    }

    public class UpdateEventDateRequest
    {
        public int Id { get; set; }
        public DateTime NewDate { get; set; }
    }
}
