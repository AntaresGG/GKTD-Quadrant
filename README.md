# QuadrantGTD

**Language / 语言 / 言語 / Sprache / Langue**

[English](README.md) | [中文](README.zh.md) | [日本語](README.ja.md) | [Deutsch](README.de.md) | [Français](README.fr.md)

---

> A four-quadrant GTD task management tool based on the Eisenhower Matrix, built with Avalonia UI for Windows.

![Version](https://img.shields.io/badge/version-1.2.0-blue)
![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11%20x64-lightgrey)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

---

## Features

### Four-Quadrant Classification (Eisenhower Matrix)

| Quadrant | Description | Strategy |
|----------|-------------|----------|
| **Q1** Important & Urgent | Crises, deadlines, emergencies | Do first |
| **Q2** Important, Not Urgent | Planning, learning, relationship building | Schedule |
| **Q3** Not Important, Urgent | Some meetings, calls, emails | Delegate |
| **Q4** Not Important, Not Urgent | Trivial tasks, time wasters | Eliminate |

### Core Features

- Drag & drop tasks across quadrants
- Project-based task organization (new in v1.2.0)
- Filter tasks by project
- Task edit / complete / delete
- Local JSON persistence with auto-save
- Automatic data format migration
- Material Design interface

---

## Quick Start

### Requirements

- Windows 10 / 11 x64
- No runtime installation needed (self-contained)
- Disk space: ~100 MB

### Installation

1. Go to [Releases](https://github.com/AntaresGG/GKTD-Quadrant/releases) and download `QuadrantGTD.exe`
2. Run directly — no installation required
3. Data is saved automatically to `%APPDATA%\QuadrantGTD\`

### Basic Usage

1. **Add task** — Click "Add Task" at the top, fill in the title
2. **Move task** — Drag a task card to the target quadrant
3. **Edit task** — Click the edit button on the card
4. **Complete task** — Click ✓ to mark as done
5. **Delete task** — Click ✗ to remove
6. **Manage projects** — Create projects from the top menu and filter tasks

---

## Tech Stack

| Component | Version |
|-----------|---------|
| Language | C# 12 |
| Runtime | .NET 8.0 LTS |
| UI Framework | Avalonia UI 11.3 |
| UI Theme | Material.Avalonia 3.8 |
| Architecture | MVVM (CommunityToolkit.Mvvm 8.2) |
| DI Container | Microsoft.Extensions.DependencyInjection 9.0 |
| Data Format | JSON (System.Text.Json) |

---

## Project Structure

```
src/
├── QuadrantGTD/
│   ├── Models/          # Data models (TaskItem, Project, AppData)
│   ├── ViewModels/      # MVVM view models
│   ├── Views/           # Avalonia XAML views and dialogs
│   ├── Services/        # Business logic and data access
│   ├── Behaviors/       # Drag-drop behavior
│   └── Assets/          # Icons and localization resources
└── QuadrantGTD.Tests/   # xUnit unit tests
```

---

## Local Development

**Prerequisites:** .NET 8.0 SDK

```bash
git clone https://github.com/AntaresGG/GKTD-Quadrant.git
cd GKTD-Quadrant

dotnet restore
dotnet run --project src/QuadrantGTD

# Run tests
dotnet test

# Publish Windows x64 single-file
dotnet publish src/QuadrantGTD -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

---

## Release History

| Version | Date | Changes |
|---------|------|---------|
| [v1.2.0](https://github.com/AntaresGG/GKTD-Quadrant/releases/tag/v1.2.0) | 2026-03-27 | Project management, button UX improvements, data format upgrade |
| v1.1.0 | — | Drag-drop, basic task CRUD |
| v1.0.0 | — | Initial release, four-quadrant layout |

See [RELEASE_NOTES.md](RELEASE_NOTES.md) for full changelog.

---

## License

MIT © 2024 AntaresGG
