using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuadrantGTD.Models;

namespace QuadrantGTD.Services;

public class TaskService : ITaskService
{
    private readonly IDataService _dataService;
    private List<TaskItem> _tasks;

    public TaskService(IDataService dataService)
    {
        _dataService = dataService;
        _tasks = new List<TaskItem>();
    }

    public async Task InitializeAsync()
    {
        var tasks = await _dataService.LoadTasksAsync();
        _tasks = tasks?.ToList() ?? new List<TaskItem>();
    }

    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
    {
        if (_tasks.Count == 0)
        {
            await InitializeAsync();
        }
        return await Task.FromResult(_tasks.AsEnumerable());
    }

    public async Task<TaskItem?> GetTaskByIdAsync(string id)
    {
        return await Task.FromResult(_tasks.FirstOrDefault(t => t.Id == id));
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem task)
    {
        if (string.IsNullOrEmpty(task.Id))
        {
            task.Id = Guid.NewGuid().ToString();
        }
        
        task.CreatedAt = DateTime.Now;
        _tasks.Add(task);
        await _dataService.SaveTasksAsync(_tasks);
        return task;
    }

    public async Task<TaskItem> UpdateTaskAsync(TaskItem task)
    {
        var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
        if (existingTask != null)
        {
            var index = _tasks.IndexOf(existingTask);
            _tasks[index] = task;
            await _dataService.SaveTasksAsync(_tasks);
        }
        return task;
    }

    public async Task<bool> DeleteTaskAsync(string id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task != null)
        {
            _tasks.Remove(task);
            await _dataService.SaveTasksAsync(_tasks);
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByQuadrantAsync(Quadrant quadrant)
    {
        return await Task.FromResult(_tasks.Where(t => t.Quadrant == quadrant));
    }

    public async Task<bool> MoveTaskToQuadrantAsync(string taskId, Quadrant newQuadrant)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
        {
            task.Quadrant = newQuadrant;
            await _dataService.SaveTasksAsync(_tasks);
            return true;
        }
        return false;
    }

    public async Task<bool> CompleteTaskAsync(string taskId)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
        {
            task.IsCompleted = true;
            task.CompletedAt = DateTime.Now;
            await _dataService.SaveTasksAsync(_tasks);
            return true;
        }
        return false;
    }
}