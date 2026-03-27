using Avalonia.Controls;
using QuadrantGTD.ViewModels;

namespace QuadrantGTD.Views;

public partial class ProjectManagementDialog : Window
{
    public ProjectManagementDialog()
    {
        InitializeComponent();
    }

    public ProjectManagementDialog(ProjectManagementDialogViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }

    public void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
}
