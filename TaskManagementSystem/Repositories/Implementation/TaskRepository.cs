using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repositories.Interfaces;

namespace TaskManagementSystem.Repositories.Implementation
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== CORE METHODS =====================

        public async Task<IEnumerable<TaskEntity>> GetAll()
        {
            var tasks = await _context.Tasks.ToListAsync();
            await ApplyAutoOverdue(tasks);
            return tasks;
        }

        public async Task<TaskEntity?> GetById(int id)
        {
            return await _context.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task Create(TaskEntity task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async Task Update(TaskEntity task)
        {
            _context.Tasks.Update(task);
            await Task.CompletedTask;
        }

        public async Task Delete(int id)
        {
            var task = await GetById(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        // ===================== USER / LIST =====================

        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
        {
            var tasks = await _context.Tasks.ToListAsync();
            await ApplyAutoOverdue(tasks);
            return tasks;
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(string userId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();

            await ApplyAutoOverdue(tasks);
            return tasks;
        }

        // ===================== SORT / FILTER / PAGING =====================

        public async Task<IEnumerable<TaskEntity>> GetAllSortedAsync(string sortField, string sortOrder)
        {
            IQueryable<TaskEntity> query = _context.Tasks;

            query = ApplySorting(query, sortField, sortOrder);

            var tasks = await query.ToListAsync();
            await ApplyAutoOverdue(tasks);

            return tasks;
        }

        public async Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)>
            GetAllFilteredSortedPagedAsync(
                string sortField,
                string sortOrder,
                int page,
                int pageSize,
                string? statusFilter,
                string? priorityFilter,
                string? searchTerm)
        {
            IQueryable<TaskEntity> query = _context.Tasks
                .Include(t => t.User);

            query = ApplyFilters(query, statusFilter, priorityFilter, searchTerm);

            var totalCount = await query.CountAsync();

            query = ApplySorting(query, sortField, sortOrder);

            var tasks = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            await ApplyAutoOverdue(tasks);

            return (tasks, totalCount);
        }

        // ===================== AUTO OVERDUE (⭐ MAIN FEATURE) =====================

        private async Task ApplyAutoOverdue(IEnumerable<TaskEntity> tasks)
        {
            bool hasChanges = false;

            foreach (var task in tasks)
            {
                if (task.DueDate.HasValue &&
                    task.DueDate.Value.Date < DateTime.Today &&
                    task.Status != "Completed" &&
                    task.Status != "Overdue")
                {
                    task.Status = "Overdue";
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                await _context.SaveChangesAsync();
            }
        }

        // ===================== HELPERS =====================

        private IQueryable<TaskEntity> ApplyFilters(
            IQueryable<TaskEntity> query,
            string? statusFilter,
            string? priorityFilter,
            string? searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                query = query.Where(t => t.Status == statusFilter);
            }

            if (!string.IsNullOrWhiteSpace(priorityFilter) &&
                int.TryParse(priorityFilter, out var priority))
            {
                query = query.Where(t => t.Priority == priority);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lower = searchTerm.ToLower();
                query = query.Where(t =>
                    (t.Title != null && t.Title.ToLower().Contains(lower)) ||
                    (t.Description != null && t.Description.ToLower().Contains(lower)));
            }

            return query;
        }

        private IQueryable<TaskEntity> ApplySorting(
            IQueryable<TaskEntity> query,
            string sortField,
            string sortOrder)
        {
            return sortField switch
            {
                "Title" => sortOrder == "asc"
                    ? query.OrderBy(t => t.Title)
                    : query.OrderByDescending(t => t.Title),

                "Status" => sortOrder == "asc"
                    ? query.OrderBy(t => t.Status)
                    : query.OrderByDescending(t => t.Status),

                "Priority" => sortOrder == "asc"
                    ? query.OrderBy(t => t.Priority)
                    : query.OrderByDescending(t => t.Priority),

                "DueDate" => sortOrder == "asc"
                    ? query.OrderBy(t => t.DueDate ?? DateTime.MaxValue)
                    : query.OrderByDescending(t => t.DueDate ?? DateTime.MinValue),

                "CreatedAt" => sortOrder == "asc"
                    ? query.OrderBy(t => t.CreatedAt)
                    : query.OrderByDescending(t => t.CreatedAt),

                "CreatedBy" => sortOrder == "asc"
                    ? query.OrderBy(t => t.User != null ? t.User.Email : "")
                    : query.OrderByDescending(t => t.User != null ? t.User.Email : ""),

                _ => query.OrderByDescending(t => t.CreatedAt)
            };
        }
    }
}
