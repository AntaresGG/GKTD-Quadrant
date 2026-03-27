# QuadrantGTD

**Language / 语言 / 言語 / Sprache / Langue**

[English](README.md) | [中文](README.zh.md) | [日本語](README.ja.md) | [Deutsch](README.de.md) | [Français](README.fr.md)

---

> Ein GTD-Aufgabenverwaltungstool mit vier Quadranten basierend auf der Eisenhower-Matrix, entwickelt mit Avalonia UI für Windows.

![Version](https://img.shields.io/badge/version-1.2.0-blue)
![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11%20x64-lightgrey)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

---

## Funktionen

### Vier-Quadranten-Klassifizierung (Eisenhower-Matrix)

| Quadrant | Beschreibung | Strategie |
|----------|-------------|-----------|
| **Q1** Wichtig & Dringend | Krisen, Fristen, Notfälle | Sofort erledigen |
| **Q2** Wichtig, nicht dringend | Planung, Lernen, Beziehungsaufbau | Einplanen |
| **Q3** Nicht wichtig, aber dringend | Manche Meetings, Anrufe, E-Mails | Delegieren |
| **Q4** Nicht wichtig, nicht dringend | Triviale Aufgaben, Zeitverschwender | Eliminieren |

### Kernfunktionen

- Aufgaben per Drag & Drop zwischen Quadranten verschieben
- Projektbasierte Aufgabenorganisation (neu in v1.2.0)
- Aufgaben nach Projekt filtern
- Aufgaben bearbeiten / abschließen / löschen
- Lokale JSON-Persistenz mit automatischem Speichern
- Automatische Datenmigration bei Formatänderungen
- Modernes Material Design Interface

---

## Schnellstart

### Systemanforderungen

- Windows 10 / 11 x64
- Keine Runtime-Installation erforderlich (eigenständig)
- Speicherplatz: ca. 100 MB

### Installation

1. Neueste `QuadrantGTD.exe` unter [Releases](https://github.com/AntaresGG/GKTD-Quadrant/releases) herunterladen
2. Direkt ausführen — keine Installation notwendig
3. Daten werden automatisch in `%APPDATA%\QuadrantGTD\` gespeichert

### Grundlegende Bedienung

1. **Aufgabe hinzufügen** — Oben auf „Aufgabe hinzufügen" klicken und Titel eingeben
2. **Aufgabe verschieben** — Aufgabenkarte in den Zielquadranten ziehen
3. **Aufgabe bearbeiten** — Bearbeitungsschaltfläche auf der Karte anklicken
4. **Aufgabe abschließen** — ✓ anklicken um als erledigt zu markieren
5. **Aufgabe löschen** — ✗ anklicken zum Entfernen
6. **Projektverwaltung** — Projekte im oberen Menü erstellen und Aufgaben filtern

---

## Technologie-Stack

| Komponente | Version |
|------------|---------|
| Sprache | C# 12 |
| Laufzeit | .NET 8.0 LTS |
| UI-Framework | Avalonia UI 11.3 |
| UI-Theme | Material.Avalonia 3.8 |
| Architektur | MVVM (CommunityToolkit.Mvvm 8.2) |
| DI-Container | Microsoft.Extensions.DependencyInjection 9.0 |
| Datenformat | JSON (System.Text.Json) |

---

## Projektstruktur

```
src/
├── QuadrantGTD/
│   ├── Models/          # Datenmodelle (TaskItem, Project, AppData)
│   ├── ViewModels/      # MVVM View Models
│   ├── Views/           # Avalonia XAML Views und Dialoge
│   ├── Services/        # Geschäftslogik und Datenzugriff
│   ├── Behaviors/       # Drag-Drop-Verhalten
│   └── Assets/          # Icons und Lokalisierungsressourcen
└── QuadrantGTD.Tests/   # xUnit Unit Tests
```

---

## Lokale Entwicklung

**Voraussetzung:** .NET 8.0 SDK

```bash
git clone https://github.com/AntaresGG/GKTD-Quadrant.git
cd GKTD-Quadrant

dotnet restore
dotnet run --project src/QuadrantGTD

# Tests ausführen
dotnet test

# Windows x64 Einzeldatei veröffentlichen
dotnet publish src/QuadrantGTD -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

---

## Versionshistorie

| Version | Datum | Änderungen |
|---------|-------|-----------|
| [v1.2.0](https://github.com/AntaresGG/GKTD-Quadrant/releases/tag/v1.2.0) | 2026-03-27 | Projektverwaltung, UX-Verbesserungen für Schaltflächen, Datenformat-Upgrade |
| v1.1.0 | — | Drag & Drop, grundlegende Aufgaben-CRUD |
| v1.0.0 | — | Erstveröffentlichung, Vier-Quadranten-Layout |

Vollständiges Änderungsprotokoll unter [RELEASE_NOTES.md](RELEASE_NOTES.md).

---

## Lizenz

MIT © 2024 AntaresGG
