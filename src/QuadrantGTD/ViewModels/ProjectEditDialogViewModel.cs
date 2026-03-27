using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuadrantGTD.Models;
using QuadrantGTD.Services;

namespace QuadrantGTD.ViewModels;

public partial class ProjectEditDialogViewModel : ViewModelBase
{
    private readonly IProjectService _projectService;
    private Project? _originalProject;

    [ObservableProperty]
    private string projectName = string.Empty;

    [ObservableProperty]
    private string projectColor = "#2196F3";

    [ObservableProperty]
    private string dialogTitle = "编辑项目";

    public event EventHandler<bool>? DialogClosed;

    public ProjectEditDialogViewModel(IProjectService projectService, Project? project = null)
    {
        _projectService = projectService;
        _originalProject = project;
    }

    public void InitializeForEdit(Project project)
    {
        _originalProject = project;
        DialogTitle = "编辑项目";
        ProjectName = project.Name;
        ProjectColor = project.Color;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(ProjectName)) return;

        if (_originalProject != null)
        {
            // 更新现有项目
            _originalProject.Name = ProjectName;
            _originalProject.Color = ProjectColor;
            await _projectService.UpdateProjectAsync(_originalProject);
        }
        else
        {
            // 创建新项目
            var newProject = new Project(ProjectName, ProjectColor);
            await _projectService.CreateProjectAsync(newProject);
        }

        DialogClosed?.Invoke(this, true);
    }

    [RelayCommand]
    private void Cancel()
    {
        DialogClosed?.Invoke(this, false);
    }
}
