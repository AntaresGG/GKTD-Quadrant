using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using QuadrantGTD.Models;

namespace QuadrantGTD.Services;

public class JsonDataService : IDataService
{
    private readonly string _dataDirectory;
    private readonly string _dataFilePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonDataService()
    {
        _dataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "QuadrantGTD"
        );

        _dataFilePath = Path.Combine(_dataDirectory, "appdata.json");

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<AppData> LoadDataAsync()
    {
        try
        {
            if (!File.Exists(_dataFilePath))
            {
                return new AppData();
            }

            var json = await File.ReadAllTextAsync(_dataFilePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new AppData();
            }

            // 检查 JSON 是数组还是对象
            var trimmedJson = json.Trim();
            if (trimmedJson.StartsWith("["))
            {
                // 旧格式：任务列表数组
                try
                {
                    var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json, _jsonOptions);
                    var data = new AppData { Tasks = tasks ?? new List<TaskItem>() };
                    // 立即保存为新格式
                    await SaveDataAsync(data);
                    return data;
                }
                catch
                {
                    return new AppData();
                }
            }
            else if (trimmedJson.StartsWith("{"))
            {
                // 新格式：AppData 对象
                try
                {
                    var data = JsonSerializer.Deserialize<AppData>(json, _jsonOptions);
                    return data ?? new AppData();
                }
                catch
                {
                    // 如果解析失败，尝试回退到旧格式处理（防万一）
                    return new AppData();
                }
            }

            return new AppData();
        }
        catch (Exception)
        {
            return new AppData();
        }
    }

    public async Task<bool> SaveDataAsync(AppData data)
    {
        try
        {
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }

            var json = JsonSerializer.Serialize(data, _jsonOptions);
            await File.WriteAllTextAsync(_dataFilePath, json);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IEnumerable<TaskItem>> LoadTasksAsync()
    {
        var data = await LoadDataAsync();
        return data.Tasks;
    }

    public async Task<bool> SaveTasksAsync(IEnumerable<TaskItem> tasks)
    {
        var data = await LoadDataAsync();
        data.Tasks = tasks.ToList();
        return await SaveDataAsync(data);
    }

    public async Task<bool> ExportTasksAsync(string filePath, IEnumerable<TaskItem> tasks)
    {
        try
        {
            var json = JsonSerializer.Serialize(tasks, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IEnumerable<TaskItem>> ImportTasksAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return new List<TaskItem>();
            }

            var json = await File.ReadAllTextAsync(filePath);
            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json, _jsonOptions);
            return tasks ?? new List<TaskItem>();
        }
        catch (Exception)
        {
            return new List<TaskItem>();
        }
    }
}
