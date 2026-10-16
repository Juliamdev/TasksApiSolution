using TaskApi.Models;
using Xunit;

namespace TaskApi.Tests;

public class TaskItemTests
{
    [Fact]
    public void TaskItem_Creation_ShouldSetDefaultValuesCorrectly()
    {
        // Act
        var task = new TaskItem
        {
            Title = "Estudar .NET 8",
            Description = "Aprender Minimal APIs"
        };

        // Assert
        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.False(task.IsCompleted);
        Assert.True(task.CreatedAt <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateTitle_ShouldReturnError_WhenTitleIsInvalid(string? invalidTitle)
    {
        // Act
        var (isValid, errorMessage) = TaskItem.ValidateTitle(invalidTitle);

        // Assert
        Assert.False(isValid);
        Assert.NotNull(errorMessage);
        Assert.Equal("O título da tarefa não pode ser vazio.", errorMessage);
    }

    [Fact]
    public void ValidateTitle_ShouldReturnTrue_WhenTitleIsValid()
    {
        // Act
        var (isValid, errorMessage) = TaskItem.ValidateTitle("Título Válido");

        // Assert
        Assert.True(isValid);
        Assert.Null(errorMessage);
    }
}