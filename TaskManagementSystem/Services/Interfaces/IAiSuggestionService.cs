namespace TaskManagementSystem.Services.Interfaces
{
    public interface IAiSuggestionService
    {
        List<string> GenerateSuggestions(string userId);
    }
}
