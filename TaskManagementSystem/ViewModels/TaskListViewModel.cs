using System.Collections.Generic;

namespace TaskManagementSystem.ViewModels
{
    public class TaskListViewModel
    {
        public List<TaskViewModel> Tasks { get; set; } = new();
    }
}
