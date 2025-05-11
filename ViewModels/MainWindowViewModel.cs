using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReflectionApp.Services;

namespace ReflectionApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly ReflectionService _reflectionService = new();
        private string _output = string.Empty;
        private string _assemblyPath = string.Empty;
        private Type? _selectedType;
        private MethodInfo? _selectedMethod;
        private bool _isTriangleSelected;

        public ObservableCollection<Type> Types { get; } = new();
        public ObservableCollection<MethodInfo> Methods { get; } = new();
        public ObservableCollection<ParameterViewModel> Parameters { get; } = new();

        public string AssemblyPath
        {
            get => _assemblyPath;
            set => SetProperty(ref _assemblyPath, value);
        }

        public string Output
        {
            get => _output;
            set => SetProperty(ref _output, value);
        }

        public Type? SelectedType
        {
            get => _selectedType;
            set => SetSelectedType(value);
        }

        public MethodInfo? SelectedMethod
        {
            get => _selectedMethod;
            set => SetSelectedMethod(value);
        }

        public bool IsTriangleSelected
        {
            get => _isTriangleSelected;
            private set => SetProperty(ref _isTriangleSelected, value);
        }

        [RelayCommand]
        private void LoadAssembly()
        {
            try
            {
                ClearAll();
                if (string.IsNullOrWhiteSpace(AssemblyPath))
                    throw new ArgumentException("Укажите путь к DLL");

                var assembly = _reflectionService.LoadAssembly(AssemblyPath) ??
                    throw new InvalidOperationException("Не удалось загрузить сборку");

                foreach (var type in _reflectionService.GetPluginTypes(assembly))
                    Types.Add(type);

                Output = $"Загружено классов: {Types.Count}";
            }
            catch (Exception ex)
            {
                Output = $"Ошибка: {ex.Message}";
            }
        }

        [RelayCommand]
        private void ExecuteMethod()
        {
            if (SelectedType == null || SelectedMethod == null)
            {
                Output = "Выберите класс и метод";
                return;
            }

            try
            {
                var instance = _reflectionService.CreateInstance(SelectedType) ??
                    throw new NullReferenceException("Не удалось создать экземпляр класса");

                var parameters = Parameters
                    .Select(p => Convert.ChangeType(p.Value, p.Type))
                    .ToArray();

                var result = _reflectionService.InvokeMethod(instance, SelectedMethod, parameters);
                Output = result?.ToString() ?? "Метод выполнен (void)";
            }
            catch (Exception ex)
            {
                Output = $"Ошибка выполнения: {ex.Message}";
            }
        }

        private void SetSelectedType(Type? type)
        {
            if (SetProperty(ref _selectedType, type))
            {
                IsTriangleSelected = type?.Name == "Triangle";
                LoadMethods();
            }
        }

        private void SetSelectedMethod(MethodInfo? method)
        {
            if (SetProperty(ref _selectedMethod, method) && method != null)
                LoadParameters();
        }

        private void LoadMethods()
        {
            Methods.Clear();
            Parameters.Clear();

            if (SelectedType == null) return;

            foreach (var method in SelectedType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                         .Where(m => !m.IsSpecialName))
                Methods.Add(method);
        }

        private void LoadParameters()
        {
            Parameters.Clear();
            if (SelectedMethod == null) return;

            foreach (var param in SelectedMethod.GetParameters())
                Parameters.Add(new ParameterViewModel
                {
                    Name = param.Name ?? "Без имени",
                    Type = param.ParameterType,
                    Value = param.ParameterType.IsValueType ?
                        Activator.CreateInstance(param.ParameterType) :
                        null
                });
        }

        private void ClearAll()
        {
            Types.Clear();
            Methods.Clear();
            Parameters.Clear();
            Output = string.Empty;
        }
    }
}