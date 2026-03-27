using Avalonia.Controls;
using Avalonia.Interactivity;
using QuadrantGTD.ViewModels;

namespace QuadrantGTD.Views;

public partial class ProjectEditDialog : Window
{
    public ProjectEditDialog()
    {
        InitializeComponent();
    }

    public ProjectEditDialog(ProjectEditDialogViewModel viewModel) : this()
    {
        DataContext = viewModel;
        viewModel.DialogClosed += OnDialogClosed;
    }

    private void OnDialogClosed(object? sender, bool result)
    {
        Close(result);
    }
}
