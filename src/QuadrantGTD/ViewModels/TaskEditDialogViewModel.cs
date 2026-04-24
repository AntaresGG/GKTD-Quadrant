using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuadrantGTD.Models;
using QuadrantGTD.Services;

namespace QuadrantGTD.ViewModels;

public partial class TaskEditDialogViewModel : ViewModelBase
{
    private readonly IProjectService? _projectService;

    [ObservableProperty]
    private string taskTitle = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private QuadrantItem? selectedQuadrantItem;

    [ObservableProperty]
    private Priority selectedPriority = Priority.Medium;

    [ObservableProperty]
    private DateTime? dueDate;

    [ObservableProperty]
    private DateTimeOffset? dueDateOffset;

    [ObservableProperty]
    private string dialogTitle = "新建任务";

    [ObservableProperty]
    private TaskItem? editingTask;

    [ObservableProperty]
    private Project? selectedProject;

    [ObservableProperty]
    private ObservableCollection<Project> projects = new();

    public List<QuadrantItem> Quadrants { get; }
    public List<PriorityItem> Priorities { get; }

    public TaskEditDialogViewModel() : this(null)
    {
    }

    public TaskEditDialogViewModel(IProjectService? projectService)
    {
        _projectService = projectService;

        Quadrants = Enum.GetValues<Quadrant>()
            .Select(q => new QuadrantItem(q, GetQuadrantName(q)))
            .ToList();

        Priorities = Enum.GetValues<Priority>()
            .Select(p => new PriorityItem(p, GetPriorityName(p)))
            .ToList();

        // 异步加载项目列表
        _ = LoadProjectsAsync();
    }

    private async Task LoadProjectsAsync()
    {
        if (_projectService == null) return;

        var allProjects = await _projectService.GetAllProjectsAsync();
        Projects.Clear();
        Projects.Add(new Project { Id = "", Name = "无项目", Color = "#9E9E9E" }); // 添加"无项目"选项
        foreach (var project in allProjects)
        {
            Projects.Add(project);
        }
    }

    public void InitializeForNewTask()
    {
        DialogTitle = LocalizationManager.Instance.CurrentLanguage == "zh-CN" ? "新建任务" : "New Task";
        EditingTask = null;
        TaskTitle = string.Empty;
        Description = string.Empty;
        SelectedQuadrantItem = Quadrants.First(q => q.Value == Quadrant.NotUrgentImportant);
        SelectedPriority = Priority.Medium;
        DueDate = null;
        DueDateOffset = null;
        SelectedProject = Projects.FirstOrDefault(); // 默认"无项目"
    }

    public void InitializeForEdit(TaskItem task)
    {
        if (task == null)
        {
            InitializeForNewTask();
            return;
        }

        DialogTitle = LocalizationManager.Instance.CurrentLanguage == "zh-CN" ? "编辑任务" : "Edit Task";
        EditingTask = task;
        TaskTitle = task.Title;
        Description = task.Description;
        SelectedQuadrantItem = Quadrants.First(q => q.Value == task.Quadrant);
        SelectedPriority = task.Priority;
        DueDate = task.DueDate;
        DueDateOffset = task.DueDate.HasValue ? new DateTimeOffset(task.DueDate.Value) : null;

        // 设置项目选择
        SelectedProject = string.IsNullOrEmpty(task.ProjectId)
            ? Projects.FirstOrDefault()
            : Projects.FirstOrDefault(p => p.Id == task.ProjectId);
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(TaskTitle))
        {
            return;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
    }

    public TaskItem GetTaskData()
    {
        return new TaskItem
        {
            Id = EditingTask?.Id ?? Guid.NewGuid().ToString(),
            Title = TaskTitle,
            Description = Description,
            Quadrant = SelectedQuadrantItem?.Value ?? Quadrant.NotUrgentImportant,
            Priority = SelectedPriority,
            DueDate = DueDateOffset?.Date,
            ProjectId = SelectedProject?.Id ?? "",
            CreatedAt = EditingTask?.CreatedAt ?? DateTime.Now,
            IsCompleted = EditingTask?.IsCompleted ?? false,
            CompletedAt = EditingTask?.CompletedAt
        };
    }

    partial void OnDueDateOffsetChanged(DateTimeOffset? value)
    {
        DueDate = value?.Date;
    }

    private static string GetQuadrantName(Quadrant quadrant)
    {
        bool isZh = LocalizationManager.Instance.CurrentLanguage == "zh-CN";
        return quadrant switch
        {
            Quadrant.UrgentImportant => isZh ? "Q1: 重要且紧急" : "Q1: Urgent & Important",
            Quadrant.NotUrgentImportant => isZh ? "Q2: 重要不紧急" : "Q2: Not Urgent & Important",
            Quadrant.UrgentNotImportant => isZh ? "Q3: 不重要但紧急" : "Q3: Urgent & Not Important",
            Quadrant.NotUrgentNotImportant => isZh ? "Q4: 不重要不紧急" : "Q4: Not Urgent & Not Important",
            _ => quadrant.ToString()
        };
    }

    private static string GetPriorityName(Priority priority)
    {
        bool isZh = LocalizationManager.Instance.CurrentLanguage == "zh-CN";
        return priority switch
        {
            Priority.Low => isZh ? "低" : "Low",
            Priority.Medium => isZh ? "中" : "Medium",
            Priority.High => isZh ? "高" : "High",
            _ => priority.ToString()
        };
    }

}

public class QuadrantItem
{
    public Quadrant Value { get; }
    public string Name { get; }

    public QuadrantItem(Quadrant value, string name)
    {
        Value = value;
        Name = name;
    }
}

public class PriorityItem
{
    public Priority Value { get; }
    public string Name { get; }

    public PriorityItem(Priority value, string name)
    {
        Value = value;
        Name = name;
    }
}

public class TaskEditResultEventArgs : EventArgs
{
    public bool IsSuccess { get; }
    public TaskItem TaskData { get; }

    public TaskEditResultEventArgs(bool isSuccess, TaskItem taskData)
    {
        IsSuccess = isSuccess;
        TaskData = taskData;
    }
}
