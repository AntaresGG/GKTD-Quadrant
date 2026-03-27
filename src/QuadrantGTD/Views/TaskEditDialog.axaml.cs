using Avalonia.Controls;
using QuadrantGTD.ViewModels;
using System.Threading.Tasks;

namespace QuadrantGTD.Views;

public partial class TaskEditDialog : Window
{
    public TaskEditDialog()
    {
        InitializeComponent();
    }

    public TaskEditDialog(TaskEditDialogViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }

    private void InitializeComponent()
    {
        Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is TaskEditDialogViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.TaskTitle))
            {
                return;
            }
            
            var taskData = vm.GetTaskData();
            Close(taskData);
        }
    }

    private void CancelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(null);
    }
}