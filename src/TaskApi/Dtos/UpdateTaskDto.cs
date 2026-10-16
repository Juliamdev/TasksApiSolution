namespace TaskApi.Dtos;

public record UpdateTaskDto(string Title, string Description, bool IsCompleted);