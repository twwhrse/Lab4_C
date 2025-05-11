using System;
using System.Drawing;

public class ParameterViewModel
{
    public string Name { get; set; } = string.Empty;
    public Type Type { get; set; } = typeof(object);
    public object? Value { get; set; }

    // Свойства для треугольника
    public Point VertexA { get; set; } = new(0, 0);
    public Point VertexB { get; set; } = new(1, 0);
    public Point VertexC { get; set; } = new(0, 1);
}