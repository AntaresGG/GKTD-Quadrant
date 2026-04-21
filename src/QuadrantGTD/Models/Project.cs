using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QuadrantGTD.Models;

/// <summary>
/// 项目实体模型
/// 用于任务的项目分类和筛选
/// </summary>
public partial class Project : ObservableObject
{
    /// <summary>
    /// 项目唯一标识符
    /// </summary>
    [ObservableProperty]
    private string id = Guid.NewGuid().ToString();

    /// <summary>
    /// 项目名称
    /// </summary>
    [ObservableProperty]
    private string name = string.Empty;

    /// <summary>
    /// 项目颜色标识（十六进制颜色码）
    /// </summary>
    [ObservableProperty]
    private string color = "#2196F3"; // Material Blue

    /// <summary>
    /// 项目创建时间
    /// </summary>
    [ObservableProperty]
    private DateTime createdAt = DateTime.Now;

    /// <summary>
    /// 项目是否已归档
    /// </summary>
    [ObservableProperty]
    private bool isArchived = false;

    /// <summary>
    /// 是否在左侧筛选列表中被选中
    /// </summary>
    [ObservableProperty]
    private bool isFilterSelected = false;

    public Project() { }

    public Project(string name, string color = "#2196F3")
    {
        Name = name;
        Color = color;
    }
}
