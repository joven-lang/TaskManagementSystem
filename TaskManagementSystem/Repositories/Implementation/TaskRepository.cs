using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repositories.Interfaces;

namespace TaskManagementSystem.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }




        public TaskEntity? GetById(int id)
        {
            return _context.Tasks.Find(id);
        }




        public IEnumerable<TaskEntity> GetAll()
        {
            return _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }






        public void Create(TaskEntity task)
        {
            task.CreatedAt = DateTime.Now;
            task.UpdatedAt = DateTime.Now;

            _context.Tasks.Add(task);
        }






        public void Update(TaskEntity task)
        {
            task.UpdatedAt = DateTime.Now;
            _context.Tasks.Update(task);
        }






        public void Delete(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }
        }







        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
