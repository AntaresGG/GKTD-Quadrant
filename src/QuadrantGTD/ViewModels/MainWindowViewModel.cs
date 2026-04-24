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

public enum ViewMode
{
    Quadrant,
    ProjectBoard,
    Archive
}

public enum ArchiveViewMode
{
    Completed,
    Deleted
}

public partial class ProjectColumn : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string color = "#2196F3";

    [ObservableProperty]
    private string? projectId;

    public ObservableCollection<TaskItem> Tasks { get; } = new();

    public ProjectColumn(string name, string color, string? projectId = null)
    {
        Name = name;
        Color = color;
        ProjectId = projectId;
    }
}

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ITaskService _taskService;
    private readonly IDataService _dataService;
    private readonly IProjectService _projectService;
    private ViewMode _lastPrimaryViewMode = ViewMode.Quadrant;

    [ObservableProperty]
    private ViewMode _currentViewMode = ViewMode.Quadrant;

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
    private ObservableCollection<TaskItem> deletedTasks = new();

    [ObservableProperty]
    private ObservableCollection<ProjectColumn> projectColumns = new();

    [ObservableProperty]
    private TaskItem? selectedTask;

    [ObservableProperty]
    private bool isLoading;

    // 项目管理相关属性
    [ObservableProperty]
    private ObservableCollection<Project> projects = new();

    [ObservableProperty]
    private HashSet<string> selectedProjectIds = new();

    [ObservableProperty]
    private bool isSidebarCollapsed = false;

    [ObservableProperty]
    private string allProjectsFilterBackground = "#BBDEFB"; // 默认选中"全部"

    [ObservableProperty]
    private bool isAllProjectsFilterSelected = true;

    [ObservableProperty]
    private ArchiveViewMode currentArchiveViewMode = ArchiveViewMode.Completed;

    public bool HasProjects => Projects.Count > 0;

    public string ArchiveButtonText
    {
        get
        {
            var lang = LocalizationManager.Instance.CurrentLanguage;
            if (CurrentViewMode != ViewMode.Archive)
            {
                return lang == "zh-CN" ? "查看已完成" : "Show Completed";
            }

            return CurrentArchiveViewMode == ArchiveViewMode.Completed
                ? (lang == "zh-CN" ? "查看已删除" : "Show Deleted")
                : (lang == "zh-CN" ? "返回主视图" : "Back To Main View");
        }
    }

    public bool IsQuadrantView => CurrentViewMode == ViewMode.Quadrant;
    public bool IsProjectBoardView => CurrentViewMode == ViewMode.ProjectBoard;
    public bool IsArchiveView => CurrentViewMode == ViewMode.Archive;
    public bool IsCompletedView => IsArchiveView && CurrentArchiveViewMode == ArchiveViewMode.Completed;
    public bool IsDeletedView => IsArchiveView && CurrentArchiveViewMode == ArchiveViewMode.Deleted;
    public bool IsQuadrantPrimarySelected => CurrentViewMode == ViewMode.Quadrant || (IsArchiveView && _lastPrimaryViewMode == ViewMode.Quadrant);
    public bool IsProjectBoardPrimarySelected => CurrentViewMode == ViewMode.ProjectBoard || (IsArchiveView && _lastPrimaryViewMode == ViewMode.ProjectBoard);

    public string ToggleLanguageButtonText => LocalizationManager.Instance.CurrentLanguage == "zh-CN" ? "EN" : "中文";

    [RelayCommand]
    private void ToggleLanguage()
    {
        var newLang = LocalizationManager.Instance.CurrentLanguage == "zh-CN" ? "en-US" : "zh-CN";
        LocalizationManager.Instance.SetLanguage(newLang);
        OnPropertyChanged(nameof(ArchiveButtonText));
        OnPropertyChanged(nameof(ToggleLanguageButtonText));
    }

    public MainWindowViewModel(ITaskService taskService, IDataService dataService, IProjectService projectService)
    {
        _taskService = taskService;
        _dataService = dataService;
        _projectService = projectService;

        InitializeAsync();
    }

    public MainWindowViewModel() : this(new TaskService(new JsonDataService()), new JsonDataService(), new ProjectService(new JsonDataService()))
    {
    }

    private async void InitializeAsync()
    {
        IsLoading = true;
        try
        {
            // 初始化服务
            if (_taskService is TaskService ts) await ts.InitializeAsync();
            if (_projectService is ProjectService ps) await ps.InitializeAsync();

            await LoadProjectsAsync();
            await LoadTasksAsync();
            
            // 只有当既没有任务也没有项目时，才添加样本数据
            if (!Q1Tasks.Any() && !Q2Tasks.Any() && !Q3Tasks.Any() && !Q4Tasks.Any() && !CompletedTasks.Any() && !Projects.Any())
            {
                await AddSampleTasksAsync();
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadTasksAsync()
    {
        try
        {
            IsLoading = true;
            var allTasks = await _taskService.GetAllTasksAsync();
            var projectLookup = Projects.ToDictionary(p => p.Id, p => p);

            foreach (var task in allTasks)
            {
                if (!string.IsNullOrEmpty(task.ProjectId) && projectLookup.TryGetValue(task.ProjectId, out var project))
                {
                    task.ProjectDisplayName = project.Name;
                    task.ProjectDisplayColor = project.Color;
                }
                else
                {
                    task.ProjectDisplayName = null;
                    task.ProjectDisplayColor = "#94A3B8";
                }
            }

            // 应用项目筛选
            var filteredTasks = (SelectedProjectIds.Count == 0
                ? allTasks
                : allTasks.Where(t => string.IsNullOrEmpty(t.ProjectId) || SelectedProjectIds.Contains(t.ProjectId))).ToList();

            Q1Tasks.Clear();
            Q2Tasks.Clear();
            Q3Tasks.Clear();
            Q4Tasks.Clear();
            CompletedTasks.Clear();
            DeletedTasks.Clear();
            ProjectColumns.Clear();

            foreach (var task in filteredTasks)
            {
                if (task.IsDeleted)
                {
                    DeletedTasks.Add(task);
                }
                else if (task.IsCompleted)
                {
                    CompletedTasks.Add(task);
                }
                else
                {
                    switch (task.Quadrant)
                    {
                        case Quadrant.UrgentImportant: Q1Tasks.Add(task); break;
                        case Quadrant.NotUrgentImportant: Q2Tasks.Add(task); break;
                        case Quadrant.UrgentNotImportant: Q3Tasks.Add(task); break;
                        case Quadrant.NotUrgentNotImportant: Q4Tasks.Add(task); break;
                    }
                }
            }

            // 填充项目看板列
            var uncompletedTasks = filteredTasks.Where(t => !t.IsCompleted && !t.IsDeleted).ToList();
            
            // 1. 创建"未分类"列 (如果存在未分类任务或已有项目)
            var uncategorizedTasks = uncompletedTasks.Where(t => string.IsNullOrEmpty(t.ProjectId)).ToList();
            if (uncategorizedTasks.Any() || Projects.Count > 0)
            {
                var uncategorizedColumn = new ProjectColumn(
                    LocalizationManager.Instance.CurrentLanguage == "zh-CN" ? "未分类" : "Uncategorized", 
                    "#94A3B8", 
                    null);
                foreach (var task in uncategorizedTasks) uncategorizedColumn.Tasks.Add(task);
                ProjectColumns.Add(uncategorizedColumn);
            }

            // 2. 为每个项目创建列
            foreach (var project in Projects)
            {
                var projectColumn = new ProjectColumn(project.Name, project.Color, project.Id);
                var projectTasks = uncompletedTasks.Where(t => t.ProjectId == project.Id).ToList();
                foreach (var task in projectTasks) projectColumn.Tasks.Add(task);
                ProjectColumns.Add(projectColumn);
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
        if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            && desktop.MainWindow is Window mainWindow)
        {
            var window = new Views.TaskEditDialog(dialog);
            var result = await window.ShowDialog<TaskItem?>(mainWindow);
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
    private void ShowQuadrantView()
    {
        SetCurrentView(ViewMode.Quadrant);
    }

    [RelayCommand]
    private void ShowProjectBoardView()
    {
        SetCurrentView(ViewMode.ProjectBoard);
    }

    [RelayCommand]
    private void ToggleArchiveView()
    {
        if (CurrentViewMode != ViewMode.Archive)
        {
            CurrentArchiveViewMode = ArchiveViewMode.Completed;
            SetCurrentView(ViewMode.Archive);
            return;
        }

        if (CurrentArchiveViewMode == ArchiveViewMode.Completed)
        {
            CurrentArchiveViewMode = ArchiveViewMode.Deleted;
            NotifyViewStateChanged();
            return;
        }

        SetCurrentView(_lastPrimaryViewMode);
    }

    [RelayCommand]
    private async Task RestoreTask(TaskItem task)
    {
        if (task == null) return;

        if (task.IsDeleted)
        {
            task.IsDeleted = false;
            task.DeletedAt = null;
        }

        if (task.IsCompleted)
        {
            task.IsCompleted = false;
            task.CompletedAt = null;
        }

        await _taskService.UpdateTaskAsync(task);
        await LoadTasksAsync();
    }

    [RelayCommand]
    private async Task PermanentlyDeleteTask(TaskItem task)
    {
        if (task != null)
        {
            await _taskService.PermanentlyDeleteTaskAsync(task.Id);
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
            IsAllProjectsFilterSelected = true;
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

            IsAllProjectsFilterSelected = SelectedProjectIds.Count == 0;
        }

        UpdateProjectFilterSelectionState();
        ApplyProjectFilter();
    }

    private async Task ShowProjectManagementDialog(ProjectManagementDialogViewModel dialog)
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            && desktop.MainWindow is Window mainWindow)
        {
            var window = new ProjectManagementDialog(dialog);
            await window.ShowDialog(mainWindow);
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

        UpdateProjectFilterSelectionState();

        // 通知HasProjects属性变更
        OnPropertyChanged(nameof(HasProjects));
    }

    private void ApplyProjectFilter()
    {
        // 触发重新加载
        _ = LoadTasksAsync();
    }

    private void UpdateProjectFilterSelectionState()
    {
        foreach (var project in Projects)
        {
            project.IsFilterSelected = SelectedProjectIds.Contains(project.Id);
        }
    }

    private void SetCurrentView(ViewMode viewMode)
    {
        if (viewMode != ViewMode.Archive)
        {
            _lastPrimaryViewMode = viewMode;
        }

        CurrentViewMode = viewMode;
        NotifyViewStateChanged();
        _ = LoadTasksAsync();
    }

    private void NotifyViewStateChanged()
    {
        OnPropertyChanged(nameof(ArchiveButtonText));
        OnPropertyChanged(nameof(IsQuadrantView));
        OnPropertyChanged(nameof(IsProjectBoardView));
        OnPropertyChanged(nameof(IsArchiveView));
        OnPropertyChanged(nameof(IsCompletedView));
        OnPropertyChanged(nameof(IsDeletedView));
        OnPropertyChanged(nameof(IsQuadrantPrimarySelected));
        OnPropertyChanged(nameof(IsProjectBoardPrimarySelected));
    }
}
