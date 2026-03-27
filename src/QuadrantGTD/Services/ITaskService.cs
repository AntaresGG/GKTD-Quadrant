using System.Collections.Generic;
using System.Threading.Tasks;
using QuadrantGTD.Models;

namespace QuadrantGTD.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    Task<TaskItem?> GetTaskByIdAsync(string id);
    Task<TaskItem> CreateTaskAsync(TaskItem task);
    Task<TaskItem> UpdateTaskAsync(TaskItem task);
    Task<bool> DeleteTaskAsync(string id);
    Task<IEnumerable<TaskItem>> GetTasksByQuadrantAsync(Quadrant quadrant);
    Task<bool> MoveTaskToQuadrantAsync(string taskId, Quadrant newQuadrant);
    Task<bool> CompleteTaskAsync(string taskId);
}