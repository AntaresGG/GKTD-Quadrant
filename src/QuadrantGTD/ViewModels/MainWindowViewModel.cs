using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuadrantGTD.Models;
using QuadrantGTD.Services;
using QuadrantGTD.Views;
using Avalonia.Controls;

namespace QuadrantGTD.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ITaskService _taskService;
    private readonly IDataService _dataService;
    private readonly IProjectService? _projectService;

    [ObservableProperty]
    private ObservableCollection<TaskItem> q1Tasks = new();

    [ObservableProperty]
    private ObservableCollection<TaskItem> q2Tasks = new();

    [ObservableProperty]
    private ObservableCollection<TaskItem> q3Tasks = new();

    [ObservableProperty]
    private ObservableCollection<TaskItem> q4Tasks = new();

    [ObservableProperty]
    private ObservableCollection<TaskItem> completedTasks = new();

    [ObservableProperty]
    private TaskItem? selectedTask;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool showCompletedView;

    // 项目管理相关属性
    [ObservableProperty]
    private ObservableCollection<Project> projects = new();

    [ObservableProperty]
    private HashSet<string> selectedProjectIds = new();

    [ObservableProperty]
    private bool isSidebarCollapsed = false;

    [ObservableProperty]
    private string allProjectsFilterBackground = "#BBDEFB"; // 默认选中"全部"

    public bool HasProjects => Projects.Count > 0;

    public string ToggleButtonText => ShowCompletedView 
        ? (LocalizationManager.Instance.CurrentLanguage == "zh-CN" ? "查看四象限" : "Show Quadrants") 
        : (LocalizationManager.Instance.CurrentLanguage == "zh-CN" ? "查看已完成" : "Show Completed");

    public string ToggleLanguageButtonText => LocalizationManager.Instance.CurrentLanguage == "zh-CN" ? "EN" : "中文";

    [RelayCommand]
    private void ToggleLanguage()
    {
        var newLang = LocalizationManager.Instance.CurrentLanguage == "zh-CN" ? "en-US" : "zh-CN";
        LocalizationManager.Instance.SetLanguage(newLang);
        OnPropertyChanged(nameof(ToggleButtonText));
        OnPropertyChanged(nameof(ToggleLanguageButtonText));
    }

    public MainWindowViewModel(ITaskService taskService, IDataService dataService, IProjectService? projectService = null)
    {
        _taskService = taskService;
        _dataService = dataService;
        _projectService = projectService;

        InitializeAsync();
    }

    public MainWindowViewModel() : this(new TaskService(new JsonDataService()), new JsonDataService(), null)
    {
    }

    private async void InitializeAsync()
    {
        await LoadTasksAsync();
        
        // Add some sample tasks for demonstration
        if (!Q1Tasks.Any() && !Q2Tasks.Any() && !Q3Tasks.Any() && !Q4Tasks.Any())
        {
            await AddSampleTasksAsync();
        }
    }

    private async Task LoadTasksAsync()
    {
        try
        {
            IsLoading = true;
            var allTasks = await _taskService.GetAllTasksAsync();

            // 应用项目筛选
            var filteredTasks = SelectedProjectIds.Count == 0
                ? allTasks
                : allTasks.Where(t => string.IsNullOrEmpty(t.ProjectId) || SelectedProjectIds.Contains(t.ProjectId));

            Q1Tasks.Clear();
            Q2Tasks.Clear();
            Q3Tasks.Clear();
            Q4Tasks.Clear();
            CompletedTasks.Clear();

            foreach (var task in filteredTasks)
            {
                if (task.IsCompleted)
                {
                    CompletedTasks.Add(task);
                }
                else
                {
                    switch (task.Quadrant)
                    {
                        case Quadrant.UrgentImportant:
                            Q1Tasks.Add(task);
                            break;
                        case Quadrant.NotUrgentImportant:
                            Q2Tasks.Add(task);
                            break;
                        case Quadrant.UrgentNotImportant:
                            Q3Tasks.Add(task);
                            break;
                        case Quadrant.NotUrgentNotImportant:
                            Q4Tasks.Add(task);
                            break;
                    }
                }
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task AddSampleTasksAsync()
    {
        var sampleTasks = new[]
        {
            new TaskItem("处理紧急客户投诉", Quadrant.UrgentImportant),
            new TaskItem("修复生产环境Bug", Quadrant.UrgentImportant),
            new TaskItem("准备明天的重要会议", Quadrant.UrgentImportant),
            
            new TaskItem("学习新的编程框架", Quadrant.NotUrgentImportant),
            new TaskItem("制定季度目标", Quadrant.NotUrgentImportant),
            new TaskItem("健身锻炼", Quadrant.NotUrgentImportant),
            new TaskItem("阅读技术书籍", Quadrant.NotUrgentImportant),
            
            new TaskItem("回复非紧急邮件", Quadrant.UrgentNotImportant),
            new TaskItem("参加例行会议", Quadrant.UrgentNotImportant),
            new TaskItem("处理日常报表", Quadrant.UrgentNotImportant),
            
            new TaskItem("浏览社交媒体", Quadrant.NotUrgentNotImportant),
            new TaskItem("整理桌面文件", Quadrant.NotUrgentNotImportant)
        };

        foreach (var task in sampleTasks)
        {
            await _taskService.CreateTaskAsync(task);
        }

        await LoadTasksAsync();
    }

    [RelayCommand]
    private async Task ShowAddTask()
    {
        var dialog = new TaskEditDialogViewModel(_projectService);
        dialog.InitializeForNewTask();

        var taskData = await ShowTaskEditDialog(dialog);
        if (taskData != null)
        {
            await _taskService.CreateTaskAsync(taskData);
            await LoadTasksAsync();
        }
    }

    [RelayCommand]
    private async Task EditTask(TaskItem task)
    {
        if (task == null) return;

        var dialog = new TaskEditDialogViewModel(_projectService);
        dialog.InitializeForEdit(task);

        var taskData = await ShowTaskEditDialog(dialog);
        if (taskData != null)
        {
            await _taskService.UpdateTaskAsync(taskData);
            await LoadTasksAsync();
        }
    }

    private async Task<TaskItem?> ShowTaskEditDialog(TaskEditDialogViewModel dialog)
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = new Views.TaskEditDialog(dialog);
            var result = await window.ShowDialog<TaskItem?>(desktop.MainWindow);
            return result;
        }
        return null;
    }


    [RelayCommand]
    private async Task DeleteTask(TaskItem task)
    {
        if (task != null)
        {
            await _taskService.DeleteTaskAsync(task.Id);
            await LoadTasksAsync();
        }
    }

    [RelayCommand]
    private async Task CompleteTask(TaskItem task)
    {
        if (task != null)
        {
            await _taskService.CompleteTaskAsync(task.Id);
            await LoadTasksAsync();
        }
    }

    [RelayCommand]
    private async Task MoveTaskToQuadrant(object parameter)
    {
        if (parameter is object[] args && args.Length == 2)
        {
            if (args[0] is TaskItem task && args[1] is Quadrant quadrant)
            {
                await _taskService.MoveTaskToQuadrantAsync(task.Id, quadrant);
                await LoadTasksAsync();
            }
        }
    }

    [RelayCommand]
    private void ToggleView()
    {
        ShowCompletedView = !ShowCompletedView;
        OnPropertyChanged(nameof(ToggleButtonText));
    }

    [RelayCommand]
    private async Task RestoreTask(TaskItem task)
    {
        if (task != null && task.IsCompleted)
        {
            task.IsCompleted = false;
            task.CompletedAt = null;
            await _taskService.UpdateTaskAsync(task);
            await LoadTasksAsync();
        }
    }

    // 项目管理命令
    [RelayCommand]
    private void ToggleSidebar()
    {
        IsSidebarCollapsed = !IsSidebarCollapsed;
    }

    [RelayCommand]
    private async Task ShowProjectManagement()
    {
        if (_projectService == null) return;

        var dialog = new ProjectManagementDialogViewModel(_projectService);
        await ShowProjectManagementDialog(dialog);
        await LoadProjectsAsync();
    }

    [RelayCommand]
    private void ToggleProjectFilter(string? projectId)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            // "全部"选项 - 清空筛选
            SelectedProjectIds.Clear();
            AllProjectsFilterBackground = "#BBDEFB";
        }
        else
        {
            AllProjectsFilterBackground = "Transparent";

            if (SelectedProjectIds.Contains(projectId))
            {
                SelectedProjectIds.Remove(projectId);
            }
            else
            {
                SelectedProjectIds.Add(projectId);
            }
        }

        ApplyProjectFilter();
    }

    private async Task ShowProjectManagementDialog(ProjectManagementDialogViewModel dialog)
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = new ProjectManagementDialog(dialog);
            await window.ShowDialog(desktop.MainWindow);
        }
    }

    private async Task LoadProjectsAsync()
    {
        if (_projectService == null) return;

        var allProjects = await _projectService.GetAllProjectsAsync();
        Projects.Clear();
        foreach (var project in allProjects)
        {
            Projects.Add(project);
        }

        // 通知HasProjects属性变更
        OnPropertyChanged(nameof(HasProjects));
    }

    private void ApplyProjectFilter()
    {
        // 触发重新加载
        _ = LoadTasksAsync();
    }
}
