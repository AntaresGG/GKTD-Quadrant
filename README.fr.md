# QuadrantGTD

**Language / 语言 / 言語 / Sprache / Langue**

[English](README.md) | [中文](README.zh.md) | [日本語](README.ja.md) | [Deutsch](README.de.md) | [Français](README.fr.md)

---

> Un outil de gestion des tâches GTD à quatre quadrants basé sur la matrice d'Eisenhower, développé avec Avalonia UI pour Windows.

![Version](https://img.shields.io/badge/version-1.2.0-blue)
![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11%20x64-lightgrey)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

---

## Fonctionnalités

### Classification en quatre quadrants (Matrice d'Eisenhower)

| Quadrant | Description | Stratégie |
|----------|-------------|-----------|
| **Q1** Important & Urgent | Crises, échéances, urgences | À faire immédiatement |
| **Q2** Important, non urgent | Planification, apprentissage, relations | Planifier |
| **Q3** Pas important, mais urgent | Certaines réunions, appels, e-mails | Déléguer |
| **Q4** Ni important, ni urgent | Tâches triviales, perte de temps | Éliminer |

### Fonctions principales

- Glisser-déposer des tâches entre les quadrants
- Organisation des tâches par projet (nouveau dans v1.2.0)
- Filtrage des tâches par projet
- Modifier / terminer / supprimer des tâches
- Persistance JSON locale avec sauvegarde automatique
- Migration automatique du format de données
- Interface moderne Material Design

---

## Démarrage rapide

### Configuration requise

- Windows 10 / 11 x64
- Aucune installation de runtime requise (autonome)
- Espace disque : environ 100 Mo

### Installation

1. Téléchargez la dernière version de `QuadrantGTD.exe` sur [Releases](https://github.com/AntaresGG/GKTD-Quadrant/releases)
2. Exécutez directement — aucune installation nécessaire
3. Les données sont automatiquement sauvegardées dans `%APPDATA%\QuadrantGTD\`

### Utilisation de base

1. **Ajouter une tâche** — Cliquez sur « Ajouter une tâche » en haut et saisissez le titre
2. **Déplacer une tâche** — Faites glisser la carte vers le quadrant cible
3. **Modifier une tâche** — Cliquez sur le bouton de modification de la carte
4. **Terminer une tâche** — Cliquez sur ✓ pour marquer comme terminé
5. **Supprimer une tâche** — Cliquez sur ✗ pour supprimer
6. **Gestion des projets** — Créez des projets depuis le menu supérieur et filtrez les tâches

---

## Stack technique

| Composant | Version |
|-----------|---------|
| Langage | C# 12 |
| Runtime | .NET 8.0 LTS |
| Framework UI | Avalonia UI 11.3 |
| Thème UI | Material.Avalonia 3.8 |
| Architecture | MVVM (CommunityToolkit.Mvvm 8.2) |
| Injection de dépendances | Microsoft.Extensions.DependencyInjection 9.0 |
| Format de données | JSON (System.Text.Json) |

---

## Structure du projet

```
src/
├── QuadrantGTD/
│   ├── Models/          # Modèles de données (TaskItem, Project, AppData)
│   ├── ViewModels/      # Modèles de vue MVVM
│   ├── Views/           # Vues et dialogues Avalonia XAML
│   ├── Services/        # Logique métier et accès aux données
│   ├── Behaviors/       # Comportement glisser-déposer
│   └── Assets/          # Icônes et ressources de localisation
└── QuadrantGTD.Tests/   # Tests unitaires xUnit
```

---

## Développement local

**Prérequis :** .NET 8.0 SDK

```bash
git clone https://github.com/AntaresGG/GKTD-Quadrant.git
cd GKTD-Quadrant

dotnet restore
dotnet run --project src/QuadrantGTD

# Exécuter les tests
dotnet test

# Publier en fichier unique Windows x64
dotnet publish src/QuadrantGTD -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

---

## Historique des versions

| Version | Date | Changements |
|---------|------|-------------|
| [v1.2.0](https://github.com/AntaresGG/GKTD-Quadrant/releases/tag/v1.2.0) | 2026-03-27 | Gestion de projets, améliorations UX des boutons, mise à niveau du format de données |
| v1.1.0 | — | Glisser-déposer, CRUD de tâches de base |
| v1.0.0 | — | Version initiale, disposition à quatre quadrants |

Voir [RELEASE_NOTES.md](RELEASE_NOTES.md) pour le journal des modifications complet.

---

## Licence

MIT © 2024 AntaresGG
