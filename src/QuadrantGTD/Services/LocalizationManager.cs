using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;

namespace QuadrantGTD.Services;

public class LocalizationManager
{
    private static LocalizationManager? _instance;
    public static LocalizationManager Instance => _instance ??= new LocalizationManager();

    private readonly Dictionary<string, ResourceInclude> _languages = new();
    private ResourceInclude? _currentResource;

    private LocalizationManager()
    {
        _languages["en-US"] = new ResourceInclude(new Uri("avares://QuadrantGTD/Assets/Languages/en-US.axaml", UriKind.Absolute))
        {
            Source = new Uri("avares://QuadrantGTD/Assets/Languages/en-US.axaml", UriKind.Absolute)
        };
        _languages["zh-CN"] = new ResourceInclude(new Uri("avares://QuadrantGTD/Assets/Languages/zh-CN.axaml", UriKind.Absolute))
        {
            Source = new Uri("avares://QuadrantGTD/Assets/Languages/zh-CN.axaml", UriKind.Absolute)
        };
    }

    public void SetLanguage(string languageCode)
    {
        if (!_languages.TryGetValue(languageCode, out var newResource))
            return;

        var app = Application.Current;
        if (app == null) return;

        if (_currentResource != null)
        {
            app.Resources.MergedDictionaries.Remove(_currentResource);
        }

        app.Resources.MergedDictionaries.Add(newResource);
        _currentResource = newResource;
        
        // Update current culture for formatting
        CultureInfo.CurrentUICulture = new CultureInfo(languageCode);
        CultureInfo.CurrentCulture = new CultureInfo(languageCode);
    }

    public string CurrentLanguage => _currentResource?.Source?.ToString().Contains("zh-CN") == true ? "zh-CN" : "en-US";
}