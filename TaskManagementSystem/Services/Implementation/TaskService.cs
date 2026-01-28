using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repositories.Interfaces;
using TaskManagementSystem.Services.Interfaces;
using TaskManagementSystem.ViewModels;

namespace TaskManagementSystem.Services.Implementation
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskViewModel>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAll(); // Add await
            var viewModels = tasks.Select(t => new TaskViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });

            return viewModels;
        }

        public async Task<TaskViewModel?> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetById(id); // Add await

            if (task == null)
                return null;

            var viewModel = new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };

            return viewModel;
        }

        public async Task CreateTaskAsync(TaskCreateViewModel model, string? userId = null)
        {
            var task = new TaskEntity
            {
                Title = model.Title,
                Description = model.Description,
                Status = "Pending",
                Priority = model.Priority,
                DueDate = model.DueDate,
                CreatedByUserId = userId
            };

            await _taskRepository.Create(task); // Add await if Create is async
            await _taskRepository.Save(); // Add await
        }

        public async Task<bool> UpdateTaskAsync(int id, TaskViewModel model)
        {
            var task = await _taskRepository.GetById(id); // Add await

            if (task == null)
                return false;

            task.Title = model.Title;
            task.Description = model.Description;
            task.Status = model.Status;
            task.Priority = model.Priority;
            task.DueDate = model.DueDate;

            await _taskRepository.Update(task); // Add await if Update is async
            await _taskRepository.Save(); // Add await

            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetById(id); // Add await

            if (task == null)
                return false;

            await _taskRepository.Delete(id); // Add await if Delete is async
            await _taskRepository.Save(); // Add await

            return true;
        }
    }
}