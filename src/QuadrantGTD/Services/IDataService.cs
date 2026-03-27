using System.Collections.Generic;
using System.Threading.Tasks;
using QuadrantGTD.Models;

namespace QuadrantGTD.Services;

public interface IDataService
{
    Task<IEnumerable<TaskItem>> LoadTasksAsync();
    Task<bool> SaveTasksAsync(IEnumerable<TaskItem> tasks);
    Task<bool> ExportTasksAsync(string filePath, IEnumerable<TaskItem> tasks);
    Task<IEnumerable<TaskItem>> ImportTasksAsync(string filePath);

    // 新增：支持项目和统一数据模型
    Task<AppData> LoadDataAsync();
    Task<bool> SaveDataAsync(AppData data);
}