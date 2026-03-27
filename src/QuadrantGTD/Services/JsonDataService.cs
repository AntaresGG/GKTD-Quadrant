using System;
using System.Collections.Generic;
using System.IO;
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

    public async Task<IEnumerable<TaskItem>> LoadTasksAsync()
    {
        try
        {
            if (!File.Exists(_dataFilePath))
            {
                return new List<TaskItem>();
            }

            var json = await File.ReadAllTextAsync(_dataFilePath);
            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json, _jsonOptions);
            return tasks ?? new List<TaskItem>();
        }
        catch (Exception)
        {
            return new List<TaskItem>();
        }
    }

    public async Task<bool> SaveTasksAsync(IEnumerable<TaskItem> tasks)
    {
        try
        {
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }

            var json = JsonSerializer.Serialize(tasks, _jsonOptions);
            await File.WriteAllTextAsync(_dataFilePath, json);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
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

    // 新增：支持统一数据模型
    public async Task<AppData> LoadDataAsync()
    {
        try
        {
            // 尝试加载新格式
            if (File.Exists(_dataFilePath))
            {
                var json = await File.ReadAllTextAsync(_dataFilePath);

                // 尝试解析为新格式 AppData
                try
                {
                    var data = JsonSerializer.Deserialize<AppData>(json, _jsonOptions);
                    if (data != null)
                    {
                        return data;
                    }
                }
                catch
                {
                    // 如果解析失败，可能是旧格式，继续迁移逻辑
                }
            }

            // 迁移旧格式（纯任务列表）
            var oldFilePath = Path.Combine(_dataDirectory, "tasks.json.old");
            if (File.Exists(_dataFilePath) && !File.Exists(oldFilePath))
            {
                try
                {
                    var oldJson = await File.ReadAllTextAsync(_dataFilePath);
                    var oldTasks = JsonSerializer.Deserialize<List<TaskItem>>(oldJson, _jsonOptions);

                    var migratedData = new AppData
                    {
                        Tasks = oldTasks ?? new List<TaskItem>(),
                        Projects = new List<Project>()
                    };

                    // 保存新格式
                    await SaveDataAsync(migratedData);

                    // 备份旧文件
                    File.Move(_dataFilePath, oldFilePath);

                    return migratedData;
                }
                catch
                {
                    // 迁移失败，返回空数据
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
}