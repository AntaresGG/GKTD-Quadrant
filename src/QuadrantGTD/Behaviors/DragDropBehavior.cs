using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using QuadrantGTD.Models;

namespace QuadrantGTD.Behaviors;

public class DragDropBehavior : Behavior<Border>
{
    public static readonly StyledProperty<TaskItem?> TaskItemProperty =
        AvaloniaProperty.Register<DragDropBehavior, TaskItem?>(nameof(TaskItem));

    public TaskItem? TaskItem
    {
        get => GetValue(TaskItemProperty);
        set => SetValue(TaskItemProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject != null)
        {
            // Use handledEventsToo: true to respect buttons handling events first
            AssociatedObject.AddHandler(InputElement.PointerPressedEvent, OnPointerPressed, RoutingStrategies.Bubble, handledEventsToo: true);
            AssociatedObject.AddHandler(InputElement.PointerMovedEvent, OnPointerMoved, RoutingStrategies.Bubble, handledEventsToo: true);
            AssociatedObject.AddHandler(InputElement.PointerReleasedEvent, OnPointerReleased, RoutingStrategies.Bubble, handledEventsToo: true);
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, OnPointerPressed);
            AssociatedObject.RemoveHandler(InputElement.PointerMovedEvent, OnPointerMoved);
            AssociatedObject.RemoveHandler(InputElement.PointerReleasedEvent, OnPointerReleased);
        }
        base.OnDetaching();
    }

    private bool _isDragging;
    private Point _startPoint;
    private DateTime _pressedTime;

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var hitElement = e.Source as Control;
        if (IsButtonOrChild(hitElement)) return;

        if (e.GetCurrentPoint(AssociatedObject).Properties.IsLeftButtonPressed)
        {
            _startPoint = e.GetPosition(AssociatedObject);
            _pressedTime = DateTime.Now;
            _isDragging = false;
        }
    }

    private bool IsButtonOrChild(Control? element)
    {
        if (element == null) return false;
        var current = element;
        while (current != null)
        {
            if (current is Button) return true;
            var parent = current.Parent as Control ?? current.GetVisualParent() as Control;
            current = parent;
        }
        return false;
    }

    private async void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var hitElement = e.Source as Control;
        if (IsButtonOrChild(hitElement)) return;

        if (e.GetCurrentPoint(AssociatedObject).Properties.IsLeftButtonPressed && !_isDragging)
        {
            var currentPoint = e.GetPosition(AssociatedObject);
            var distance = Point.Distance(_startPoint, currentPoint);
            var timeSincePressed = DateTime.Now - _pressedTime;

            if (distance > 5 && timeSincePressed.TotalMilliseconds > 100 && TaskItem != null)
            {
                _isDragging = true;
                var dataObject = new DataObject();
                dataObject.Set("TaskItem", TaskItem);
                
                await DragDrop.DoDragDrop(e, dataObject, DragDropEffects.Move);
                _isDragging = false;
            }
        }
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isDragging = false;
    }
}

public class DropTargetBehavior : Behavior<Border>
{
    public static readonly StyledProperty<Quadrant> TargetQuadrantProperty =
        AvaloniaProperty.Register<DropTargetBehavior, Quadrant>(nameof(TargetQuadrant));

    public Quadrant TargetQuadrant
    {
        get => GetValue(TargetQuadrantProperty);
        set => SetValue(TargetQuadrantProperty, value);
    }

    public static readonly StyledProperty<ICommand?> MoveTaskCommandProperty =
        AvaloniaProperty.Register<DropTargetBehavior, ICommand?>(nameof(MoveTaskCommand));

    public ICommand? MoveTaskCommand
    {
        get => GetValue(MoveTaskCommandProperty);
        set => SetValue(MoveTaskCommandProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject != null)
        {
            DragDrop.SetAllowDrop(AssociatedObject, true);
            AssociatedObject.AddHandler(DragDrop.DragOverEvent, OnDragOver);
            AssociatedObject.AddHandler(DragDrop.DropEvent, OnDrop);
            AssociatedObject.AddHandler(DragDrop.DragEnterEvent, OnDragEnter);
            AssociatedObject.AddHandler(DragDrop.DragLeaveEvent, OnDragLeave);
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.RemoveHandler(DragDrop.DragOverEvent, OnDragOver);
            AssociatedObject.RemoveHandler(DragDrop.DropEvent, OnDrop);
            AssociatedObject.RemoveHandler(DragDrop.DragEnterEvent, OnDragEnter);
            AssociatedObject.RemoveHandler(DragDrop.DragLeaveEvent, OnDragLeave);
        }
        base.OnDetaching();
    }

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains("TaskItem"))
            e.DragEffects = DragDropEffects.Move;
        else
            e.DragEffects = DragDropEffects.None;
        e.Handled = true;
    }

    private void OnDragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains("TaskItem") && AssociatedObject != null)
            AssociatedObject.Opacity = 0.8;
    }

    private void OnDragLeave(object? sender, DragEventArgs e)
    {
        if (AssociatedObject != null)
            AssociatedObject.Opacity = 1.0;
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        if (AssociatedObject != null)
            AssociatedObject.Opacity = 1.0;

        if (e.Data.Get("TaskItem") is TaskItem taskItem && MoveTaskCommand != null)
        {
            var parameters = new object[] { taskItem, TargetQuadrant };
            if (MoveTaskCommand.CanExecute(parameters))
            {
                MoveTaskCommand.Execute(parameters);
            }
        }
        e.Handled = true;
    }
}