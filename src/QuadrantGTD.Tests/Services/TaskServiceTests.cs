using FluentAssertions;
using Moq;
using QuadrantGTD.Models;
using QuadrantGTD.Services;

namespace QuadrantGTD.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<IDataService> _mockDataService;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _mockDataService = new Mock<IDataService>();
        _taskService = new TaskService(_mockDataService.Object);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldCreateTaskWithId()
    {
        // Arrange
        var task = new TaskItem("Test Task", Quadrant.UrgentImportant);
        _mockDataService.Setup(x => x.SaveTasksAsync(It.IsAny<IEnumerable<TaskItem>>()))
                       .ReturnsAsync(true);

        // Act
        var result = await _taskService.CreateTaskAsync(task);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrEmpty();
        result.Title.Should().Be("Test Task");
        result.Quadrant.Should().Be(Quadrant.UrgentImportant);
    }

    [Fact]
    public async Task MoveTaskToQuadrantAsync_ShouldUpdateTaskQuadrant()
    {
        // Arrange
        var task = new TaskItem("Test Task", Quadrant.UrgentImportant);
        await _taskService.CreateTaskAsync(task);
        _mockDataService.Setup(x => x.SaveTasksAsync(It.IsAny<IEnumerable<TaskItem>>()))
                       .ReturnsAsync(true);

        // Act
        var result = await _taskService.MoveTaskToQuadrantAsync(task.Id, Quadrant.NotUrgentImportant);

        // Assert
        result.Should().BeTrue();
        task.Quadrant.Should().Be(Quadrant.NotUrgentImportant);
    }

    [Fact]
    public async Task CompleteTaskAsync_ShouldMarkTaskAsCompleted()
    {
        // Arrange
        var task = new TaskItem("Test Task", Quadrant.UrgentImportant);
        await _taskService.CreateTaskAsync(task);
        _mockDataService.Setup(x => x.SaveTasksAsync(It.IsAny<IEnumerable<TaskItem>>()))
                       .ReturnsAsync(true);

        // Act
        var result = await _taskService.CompleteTaskAsync(task.Id);

        // Assert
        result.Should().BeTrue();
        task.IsCompleted.Should().BeTrue();
        task.CompletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldRemoveTask()
    {
        // Arrange
        var task = new TaskItem("Test Task", Quadrant.UrgentImportant);
        await _taskService.CreateTaskAsync(task);
        _mockDataService.Setup(x => x.SaveTasksAsync(It.IsAny<IEnumerable<TaskItem>>()))
                       .ReturnsAsync(true);

        // Act
        var result = await _taskService.DeleteTaskAsync(task.Id);

        // Assert
        result.Should().BeTrue();
        var allTasks = await _taskService.GetAllTasksAsync();
        allTasks.Should().NotContain(t => t.Id == task.Id);
    }
}