using System;

namespace SampleLibrary.PluginClasses
{
    public class CalculatorPlugin : IPlugin
    {
        public string Name => "Калькулятор";
        public string Description => "Производит математические операции";

        // Математические операции
        public int Add(int a, int b) => a + b;

        public double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Деление на ноль невозможно");
            return a / b;
        }

        // Новые методы
        public int Multiply(int a, int b) => a * b;

        public double Power(double a, double exponent)
        {
            if (a == 0 && exponent < 0)
                throw new ArgumentException("Невозможно возвести 0 в отрицательную степень");
            return Math.Pow(a, exponent);
        }

        // Стандартные методы .NET
        public override string ToString()
        {
            return $"Плагин: {Name} ({Description})";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description);
        }
    }
}