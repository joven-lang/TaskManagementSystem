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


        // ========================================
        // EXISTING METHODS (keep as-is)
        // ========================================

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

        // ========================================
        // NEW METHODS FOR DASHBOARD
        // ========================================

        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(string userId)
        {
            // OPTION 1: If TaskEntity has CreatedByUserId property
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.CreatedByUserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            // OPTION 2: If tasks are shared (no user filter)
            // return await GetAllTasksAsync();
        }
    }
}