namespace TaskManagementSystem.Helpers
{
    public static class TaskDeadlineHelper
    {
        public static string GetDeadlineClass(DateTime? dueDate, string status)
        {
            if (status == "Completed" || dueDate == null)
                return "";

            var today = DateTime.Today;

            if (dueDate.Value.Date < today)
                return "deadline-overdue";

            if (dueDate.Value.Date == today)
                return "deadline-today";

            if (dueDate.Value.Date == today.AddDays(1))
                return "deadline-tomorrow";

            return "";
        }

        public static string GetDeadlineLabel(DateTime? dueDate, string status)
        {
            if (status == "Completed" || dueDate == null)
                return "";

            var today = DateTime.Today;

            if (dueDate.Value.Date < today)
                return "Overdue";

            if (dueDate.Value.Date == today)
                return "Due Today";

            if (dueDate.Value.Date == today.AddDays(1))
                return "Due Tomorrow";

            return "";
        }
    }
}
