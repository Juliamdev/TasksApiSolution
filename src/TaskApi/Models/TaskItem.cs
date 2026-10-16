namespace TaskApi.Models;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public static (bool IsValid, string? ErrorMessage) ValidateTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return (false, "O título da tarefa não pode ser vazio.");
        }

        return (true, null);
    }
}