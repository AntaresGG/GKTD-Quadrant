using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QuadrantGTD.Models;

public partial class TaskItem : ObservableObject
{
    [ObservableProperty]
    private string id = Guid.NewGuid().ToString();

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private Quadrant quadrant = Quadrant.NotUrgentImportant;

    [ObservableProperty]
    private Priority priority = Priority.Medium;

    [ObservableProperty]
    private bool isCompleted = false;

    [ObservableProperty]
    private bool isDeleted = false;

    [ObservableProperty]
    private DateTime createdAt = DateTime.Now;

    [ObservableProperty]
    private DateTime? completedAt;

    [ObservableProperty]
    private DateTime? deletedAt;

    [ObservableProperty]
    private DateTime? dueDate;

    [ObservableProperty]
    private string? tags;

    [ObservableProperty]
    private string? projectId;

    public TaskItem()
    {
    }

    public TaskItem(string title, Quadrant quadrant)
    {
        Title = title;
        Quadrant = quadrant;
    }
}
