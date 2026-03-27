# QuadrantGTD

**Language / 语言 / 言語 / Sprache / Langue**

[English](README.md) | [中文](README.zh.md) | [日本語](README.ja.md) | [Deutsch](README.de.md) | [Français](README.fr.md)

---

> 基于艾森豪威尔矩阵的四象限 GTD 任务管理工具，采用 Avalonia UI 构建，支持 Windows 原生运行。

![Version](https://img.shields.io/badge/version-1.2.0-blue)
![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11%20x64-lightgrey)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 功能特性

### 四象限分类（艾森豪威尔矩阵）

| 象限 | 描述 | 行动策略 |
|------|------|----------|
| **Q1** 重要且紧急 | 危机、截止日期、突发事件 | 立即处理 |
| **Q2** 重要不紧急 | 规划、学习、关系维护 | 安排时间 |
| **Q3** 不重要但紧急 | 部分会议、电话、邮件 | 委托他人 |
| **Q4** 不重要不紧急 | 娱乐、无效社交 | 消除或推后 |

### 核心功能

- 任务拖拽跨象限移动
- 项目维度分类管理（v1.2.0 新增）
- 按项目筛选任务视图
- 任务编辑 / 完成 / 删除操作
- 本地 JSON 数据持久化，自动保存
- 自动迁移历史数据格式
- Material Design 现代界面风格

---

## 快速开始

### 系统要求

- Windows 10 / 11 x64
- 无需安装运行时（自包含发布）
- 磁盘空间：约 100 MB

### 安装

1. 前往 [Releases](https://github.com/AntaresGG/GKTD-Quadrant/releases) 下载最新版 `QuadrantGTD.exe`
2. 双击直接运行，无需安装
3. 数据自动保存至 `%APPDATA%\QuadrantGTD\`

### 基本操作

1. **添加任务** — 点击顶部"添加任务"按钮，填写标题后确认
2. **移动任务** — 拖拽任务卡片到目标象限
3. **编辑任务** — 点击卡片上的编辑按钮
4. **完成任务** — 点击 ✓ 按钮标记完成
5. **删除任务** — 点击 ✗ 按钮删除
6. **项目管理** — 顶部菜单创建项目，按项目筛选任务

---

## 技术栈

| 组件 | 版本 |
|------|------|
| 语言 | C# 12 |
| 运行时 | .NET 8.0 LTS |
| UI 框架 | Avalonia UI 11.3 |
| UI 主题 | Material.Avalonia 3.8 |
| 架构模式 | MVVM (CommunityToolkit.Mvvm 8.2) |
| 依赖注入 | Microsoft.Extensions.DependencyInjection 9.0 |
| 数据格式 | JSON (System.Text.Json) |

---

## 项目结构

```
src/
├── QuadrantGTD/
│   ├── Models/          # 数据模型 (TaskItem, Project, AppData)
│   ├── ViewModels/      # MVVM 视图模型
│   ├── Views/           # Avalonia XAML 视图与对话框
│   ├── Services/        # 业务逻辑与数据访问层
│   ├── Behaviors/       # 拖拽行为
│   └── Assets/          # 图标与多语言资源
└── QuadrantGTD.Tests/   # xUnit 单元测试
```

---

## 本地开发

**前置要求：** .NET 8.0 SDK

```bash
git clone https://github.com/AntaresGG/GKTD-Quadrant.git
cd GKTD-Quadrant

dotnet restore
dotnet run --project src/QuadrantGTD

# 运行测试
dotnet test

# 发布 Windows x64 单文件
dotnet publish src/QuadrantGTD -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

---

## 版本历史

| 版本 | 日期 | 主要变更 |
|------|------|----------|
| [v1.2.0](https://github.com/AntaresGG/GKTD-Quadrant/releases/tag/v1.2.0) | 2026-03-27 | 项目管理功能、按钮交互优化、数据格式升级 |
| v1.1.0 | — | 拖拽功能、基础任务 CRUD |
| v1.0.0 | — | 初始版本，四象限布局 |

详细变更记录见 [RELEASE_NOTES.md](RELEASE_NOTES.md)

---

## License

MIT © 2024 AntaresGG
