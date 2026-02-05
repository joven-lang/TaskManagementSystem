public class NotificationViewModel
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string TaskTitle { get; set; }

    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }

    // UI helpers
    public string TypeLabel { get; set; }   // Due Today / Due Tomorrow / Overdue
    public string TypeClass { get; set; }   // today / tomorrow / overdue
    public string Icon { get; set; }        // bi-calendar / bi-exclamation
}
