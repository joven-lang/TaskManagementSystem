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





        public Task<IEnumerable<TaskViewModel>> GetAllTasksAsync()
        {
            var tasks = _taskRepository.GetAll();

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

            return Task.FromResult(viewModels);
        }

        public Task<TaskViewModel?> GetTaskByIdAsync(int id)
        {
            var task = _taskRepository.GetById(id);



            if (task == null)
                return Task.FromResult<TaskViewModel?>(null);

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

            return Task.FromResult<TaskViewModel?>(viewModel);
        }

        public Task CreateTaskAsync(TaskCreateViewModel model, string? userId = null)
        {
            var task = new TaskEntity
            {
                Title = model.Title,
                Description = model.Description,
                Status = "Pending",
                Priority = model.Priority,
                DueDate = model.DueDate,
                CreatedByUserId = userId // Track who created it
            };

            _taskRepository.Create(task);
            _taskRepository.Save();

            return Task.CompletedTask;
        }



        public Task<bool> UpdateTaskAsync(int id, TaskViewModel model)
        {
            var task = _taskRepository.GetById(id);


            if (task == null)
                return Task.FromResult(false);

            task.Title = model.Title;
            task.Description = model.Description;
            task.Status = model.Status;
            task.Priority = model.Priority;
            task.DueDate = model.DueDate;

            _taskRepository.Update(task);
            _taskRepository.Save();

            return Task.FromResult(true);
        }

        public Task<bool> DeleteTaskAsync(int id)
        {
            var task = _taskRepository.GetById(id);
            if (task == null)
                return Task.FromResult(false);

            _taskRepository.Delete(id);
            _taskRepository.Save();

            return Task.FromResult(true);
        }
    }
}