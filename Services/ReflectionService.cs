using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SampleLibrary;
using SampleLibrary.PluginClasses;

namespace ReflectionApp.Services
{
    public class ReflectionService
    {
        public Assembly? LoadAssembly(string path)
        {
            try { return Assembly.LoadFrom(path); }
            catch { return null; }
        }

        public IEnumerable<Type> GetPluginTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IPlugin).IsAssignableFrom(t));
        }

        public object? CreateInstance(Type type)
        {
            try
            {
                if (type == typeof(Triangle))
                {
                    return new Triangle(); // Используем конструктор по умолчанию
                }
                return Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
        }

        public object? InvokeMethod(object instance, MethodInfo method, object?[] parameters)
        {
            try
            {
                return method.Invoke(instance, parameters);
            }
            catch (Exception ex)
            {
                return $"Ошибка: {ex.InnerException?.Message ?? ex.Message}";
            }
        }
    }
}