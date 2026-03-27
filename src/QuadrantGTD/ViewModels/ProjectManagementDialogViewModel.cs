using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuadrantGTD.Models;
using QuadrantGTD.Services;
using QuadrantGTD.Views;

namespace QuadrantGTD.ViewModels;

public partial class ProjectManagementDialogViewModel : ViewModelBase
{
    private readonly IProjectService _projectService;

    [ObservableProperty]
    private ObservableCollection<Project> projects = new();

    [ObservableProperty]
    private Project? selectedProject;

    [ObservableProperty]
    private string newProjectName = string.Empty;

    [ObservableProperty]
    private string newProjectColor = "#2196F3";

    [ObservableProperty]
    private bool isEditing = false;

    [ObservableProperty]
    private Project? editingProject;

    public ProjectManagementDialogViewModel(IProjectService projectService)
    {
        _projectService = projectService;
        _ = LoadProjectsAsync();
    }

    private async Task LoadProjectsAsync()
    {
        var allProjects = await _projectService.GetAllProjectsAsync();
        Projects.Clear();
        foreach (var project in allProjects)
        {
            Projects.Add(project);
        }
    }

    [RelayCommand]
    private async Task AddProject()
    {
        if (string.IsNullOrWhiteSpace(NewProjectName)) return;

        var project = new Project(NewProjectName, NewProjectColor);
        await _projectService.CreateProjectAsync(project);

        NewProjectName = string.Empty;
        NewProjectColor = "#2196F3";

        await LoadProjectsAsync();
    }

    [RelayCommand]
    private async Task EditProject(Project? project)
    {
        if (project == null) return;

        // 打开编辑对话框
        var dialog = new ProjectEditDialogViewModel(_projectService, project);
        dialog.InitializeForEdit(project);

        if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = new ProjectEditDialog(dialog);
            await window.ShowDialog(desktop.MainWindow);

            // 刷新列表
            await LoadProjectsAsync();
        }
    }

    [RelayCommand]
    private async Task DeleteProject(Project? project)
    {
        if (project != null)
        {
            await _projectService.DeleteProjectAsync(project.Id);
            await LoadProjectsAsync();
        }
    }

    [RelayCommand]
    private void Close()
    {
        // 关闭对话框的逻辑将由视图处理
    }
}
