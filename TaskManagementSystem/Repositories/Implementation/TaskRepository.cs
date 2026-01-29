using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
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

        public async Task<IEnumerable<TaskEntity>> GetAll()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskEntity?> GetById(int id)
        {
            return await _context.Tasks.FindAsync(id);
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

        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(string userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetAllSortedAsync(string sortField, string sortOrder)
        {
            IQueryable<TaskEntity> query = _context.Tasks;

            query = ApplySorting(query, sortField, sortOrder);

            return await query.ToListAsync();
        }

        public async Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetAllFilteredSortedPagedAsync(
            string sortField,
            string sortOrder,
            int page,
            int pageSize,
            string? statusFilter,
            string? priorityFilter,
            string? searchTerm)
        {
            IQueryable<TaskEntity> query = _context.Tasks;

            // Apply filters
            query = ApplyFilters(query, statusFilter, priorityFilter, searchTerm);

            // Get total count AFTER filtering but BEFORE pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = ApplySorting(query, sortField, sortOrder);

            // Apply pagination
            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var tasks = await query.ToListAsync();

            return (tasks, totalCount);
        }

        private IQueryable<TaskEntity> ApplyFilters(
            IQueryable<TaskEntity> query,
            string? statusFilter,
            string? priorityFilter,
            string? searchTerm)
        {
            // Status filter
            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                query = query.Where(t => t.Status == statusFilter);
            }

            // Priority filter
            if (!string.IsNullOrWhiteSpace(priorityFilter) && int.TryParse(priorityFilter, out var priority))
            {
                query = query.Where(t => t.Priority == priority);
            }

            // Search term (searches in Title and Description)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerSearchTerm = searchTerm.ToLower();
                query = query.Where(t =>
                    (t.Title != null && t.Title.ToLower().Contains(lowerSearchTerm)) ||
                    (t.Description != null && t.Description.ToLower().Contains(lowerSearchTerm))
                );
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

                _ => query.OrderByDescending(t => t.CreatedAt)
            };
        }
    }
}