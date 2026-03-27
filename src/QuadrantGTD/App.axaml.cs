using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using QuadrantGTD.ViewModels;
using QuadrantGTD.Views;
using QuadrantGTD.Services;

namespace QuadrantGTD;

public partial class App : Application
{
    public IServiceProvider? ServiceProvider { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        LocalizationManager.Instance.SetLanguage("zh-CN");
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Configure dependency injection
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            var mainWindowViewModel = ServiceProvider.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Register services
        services.AddSingleton<IDataService, JsonDataService>();
        services.AddSingleton<ITaskService, TaskService>();
        services.AddSingleton<IProjectService, ProjectService>();

        // Register ViewModels
        services.AddTransient<MainWindowViewModel>();

        // Initialize TaskService
        services.AddSingleton<TaskService>(provider =>
        {
            var dataService = provider.GetRequiredService<IDataService>();
            var taskService = new TaskService(dataService);
            taskService.InitializeAsync().Wait();
            return taskService;
        });

        // Initialize ProjectService
        services.AddSingleton<ProjectService>(provider =>
        {
            var dataService = provider.GetRequiredService<IDataService>();
            var projectService = new ProjectService(dataService);
            projectService.InitializeAsync().Wait();
            return projectService;
        });
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}