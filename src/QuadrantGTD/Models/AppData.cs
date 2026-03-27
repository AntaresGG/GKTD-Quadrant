using System.Collections.Generic;

namespace QuadrantGTD.Models;

/// <summary>
/// 应用数据容器
/// 统一管理所有持久化数据
/// </summary>
public class AppData
{
    /// <summary>
    /// 所有任务列表
    /// </summary>
    public List<TaskItem> Tasks { get; set; } = new();

    /// <summary>
    /// 所有项目列表
    /// </summary>
    public List<Project> Projects { get; set; } = new();
}
