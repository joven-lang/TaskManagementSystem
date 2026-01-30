using System;

namespace TaskManagementSystem.ViewModels
{
    public class CalendarEventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string BackgroundColor { get; set; } = "#0d6efd";
        public string BorderColor { get; set; } = "#0d6efd";
        public string TextColor { get; set; } = "#ffffff";
        public bool AllDay { get; set; } = true;
        public string Status { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string? Description { get; set; }
        public string Url { get; set; } = string.Empty;

        // Extended properties for display
        public Dictionary<string, object> ExtendedProps { get; set; } = new();
    }
}