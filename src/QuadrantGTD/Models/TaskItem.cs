using System;
using System.Text.Json.Serialization;
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

    [ObservableProperty]
    [JsonIgnore]
    private string? projectDisplayName;

    [ObservableProperty]
    [JsonIgnore]
    private string projectDisplayColor = "#94A3B8";

    [JsonIgnore]
    public bool HasProject => !string.IsNullOrWhiteSpace(ProjectDisplayName);

    [JsonIgnore]
    public bool HasDueDate => DueDate.HasValue;

    partial void OnDueDateChanged(DateTime? value)
    {
        OnPropertyChanged(nameof(HasDueDate));
    }

    partial void OnProjectDisplayNameChanged(string? value)
    {
        OnPropertyChanged(nameof(HasProject));
    }

    public TaskItem()
    {
    }

    public TaskItem(string title, Quadrant quadrant)
    {
        Title = title;
        Quadrant = quadrant;
    }
}
