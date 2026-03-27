# QuadrantGTD

**Language / 语言 / 言語 / Sprache / Langue**

[English](README.md) | [中文](README.zh.md) | [日本語](README.ja.md) | [Deutsch](README.de.md) | [Français](README.fr.md)

---

> アイゼンハワーマトリクスに基づく四象限 GTD タスク管理ツール。Avalonia UI で構築された Windows ネイティブアプリケーションです。

![Version](https://img.shields.io/badge/version-1.2.0-blue)
![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11%20x64-lightgrey)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 機能

### 四象限分類（アイゼンハワーマトリクス）

| 象限 | 説明 | 対応策 |
|------|------|--------|
| **Q1** 重要かつ緊急 | 危機・締め切り・突発事態 | 今すぐ対応 |
| **Q2** 重要だが緊急でない | 計画・学習・関係構築 | スケジュールを立てる |
| **Q3** 重要でないが緊急 | 一部の会議・電話・メール | 他者に委任 |
| **Q4** 重要でも緊急でもない | 雑務・時間の無駄 | 排除する |

### 主な機能

- タスクのドラッグ＆ドロップによる象限間移動
- プロジェクト単位のタスク管理（v1.2.0 新機能）
- プロジェクトによるタスクフィルタリング
- タスクの編集 / 完了 / 削除
- ローカル JSON による自動保存
- 旧データ形式の自動マイグレーション
- Material Design モダン UI

---

## クイックスタート

### システム要件

- Windows 10 / 11 x64
- ランタイムのインストール不要（自己完結型）
- ディスク容量：約 100 MB

### インストール

1. [Releases](https://github.com/AntaresGG/GKTD-Quadrant/releases) から最新の `QuadrantGTD.exe` をダウンロード
2. ダブルクリックで直接起動（インストール不要）
3. データは `%APPDATA%\QuadrantGTD\` に自動保存されます

### 基本操作

1. **タスク追加** — 上部の「タスク追加」ボタンをクリックしてタイトルを入力
2. **タスク移動** — タスクカードを目的の象限へドラッグ
3. **タスク編集** — カード上の編集ボタンをクリック
4. **完了マーク** — ✓ ボタンをクリック
5. **タスク削除** — ✗ ボタンをクリック
6. **プロジェクト管理** — 上部メニューからプロジェクトを作成し、タスクをフィルタリング

---

## 技術スタック

| コンポーネント | バージョン |
|----------------|-----------|
| 言語 | C# 12 |
| ランタイム | .NET 8.0 LTS |
| UI フレームワーク | Avalonia UI 11.3 |
| UI テーマ | Material.Avalonia 3.8 |
| アーキテクチャ | MVVM (CommunityToolkit.Mvvm 8.2) |
| DI コンテナ | Microsoft.Extensions.DependencyInjection 9.0 |
| データ形式 | JSON (System.Text.Json) |

---

## プロジェクト構成

```
src/
├── QuadrantGTD/
│   ├── Models/          # データモデル (TaskItem, Project, AppData)
│   ├── ViewModels/      # MVVM ビューモデル
│   ├── Views/           # Avalonia XAML ビューとダイアログ
│   ├── Services/        # ビジネスロジックとデータアクセス
│   ├── Behaviors/       # ドラッグ＆ドロップ動作
│   └── Assets/          # アイコンとローカライズリソース
└── QuadrantGTD.Tests/   # xUnit 単体テスト
```

---

## ローカル開発

**前提条件：** .NET 8.0 SDK

```bash
git clone https://github.com/AntaresGG/GKTD-Quadrant.git
cd GKTD-Quadrant

dotnet restore
dotnet run --project src/QuadrantGTD

# テスト実行
dotnet test

# Windows x64 単一ファイルとして発行
dotnet publish src/QuadrantGTD -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

---

## バージョン履歴

| バージョン | 日付 | 主な変更内容 |
|------------|------|-------------|
| [v1.2.0](https://github.com/AntaresGG/GKTD-Quadrant/releases/tag/v1.2.0) | 2026-03-27 | プロジェクト管理機能、ボタン UX 改善、データ形式アップグレード |
| v1.1.0 | — | ドラッグ＆ドロップ、基本タスク CRUD |
| v1.0.0 | — | 初期リリース、四象限レイアウト |

詳細な変更履歴は [RELEASE_NOTES.md](RELEASE_NOTES.md) を参照してください。

---

## License

MIT © 2024 AntaresGG
