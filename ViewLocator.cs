using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReflectionApp.ViewModels;

namespace ReflectionApp;

public class ViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        if (data is null)
            return new TextBlock { Text = "Null ViewModel" };

        var viewName = data.GetType().FullName!
            .Replace("ViewModels", "Views")
            .Replace("ViewModel", "View");

        var viewType = Type.GetType(viewName);

        return viewType != null
            ? (Control)Activator.CreateInstance(viewType)!
            : new TextBlock { Text = $"Not Found: {viewName}" };
    }

    public bool Match(object? data) => data is ViewModelBase;
}