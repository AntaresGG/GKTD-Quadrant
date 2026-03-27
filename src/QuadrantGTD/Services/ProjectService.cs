using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuadrantGTD.Models;

namespace QuadrantGTD.Services;

public class ProjectService : IProjectService
{
    private readonly IDataService _dataService;
    private List<Project> _projects;

    public ProjectService(IDataService dataService)
    {
        _dataService = dataService;
        _projects = new List<Project>();
    }

    public async Task InitializeAsync()
    {
        var data = await _dataService.LoadDataAsync();
        _projects = data?.Projects?.ToList() ?? new List<Project>();
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        if (_projects.Count == 0)
        {
            await InitializeAsync();
        }
        return await Task.FromResult(_projects.AsEnumerable());
    }

    public async Task<Project?> GetProjectByIdAsync(string id)
    {
        if (_projects.Count == 0)
        {
            await InitializeAsync();
        }
        return await Task.FromResult(_projects.FirstOrDefault(p => p.Id == id));
    }

    public async Task<Project> CreateProjectAsync(Project project)
    {
        if (string.IsNullOrEmpty(project.Id))
        {
            project.Id = Guid.NewGuid().ToString();
        }

        project.CreatedAt = DateTime.Now;
        _projects.Add(project);
        await SaveAsync();
        return project;
    }

    public async Task<Project> UpdateProjectAsync(Project project)
    {
        var existing = _projects.FirstOrDefault(p => p.Id == project.Id);
        if (existing != null)
        {
            var index = _projects.IndexOf(existing);
            _projects[index] = project;
            await SaveAsync();
        }
        return project;
    }

    public async Task<bool> DeleteProjectAsync(string id)
    {
        var project = _projects.FirstOrDefault(p => p.Id == id);
        if (project != null)
        {
            _projects.Remove(project);
            await SaveAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
    {
        if (_projects.Count == 0)
        {
            await InitializeAsync();
        }
        return await Task.FromResult(_projects.Where(p => !p.IsArchived));
    }

    private async Task SaveAsync()
    {
        var data = await _dataService.LoadDataAsync();
        data.Projects = _projects;
        await _dataService.SaveDataAsync(data);
    }
}
