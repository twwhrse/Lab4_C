using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleLibrary.PluginClasses
{
    public class Triangle : IPlugin
    {
        public string Name => "Треугольник";
        public string Description => "Вычисляет параметры треугольника по координатам вершин";

        public Point VertexA { get; set; }
        public Point VertexB { get; set; }
        public Point VertexC { get; set; }

        public Triangle()
        {
            // Конструктор по умолчанию для рефлексии
            VertexA = new Point(0, 0);
            VertexB = new Point(1, 0);
            VertexC = new Point(0, 1);
        }

        public double CalculateArea()
        {
            double sideAB = Distance(VertexA, VertexB);
            double sideBC = Distance(VertexB, VertexC);
            double sideCA = Distance(VertexC, VertexA);

            double p = (sideAB + sideBC + sideCA) / 2;
            return Math.Sqrt(p * (p - sideAB) * (p - sideBC) * (p - sideCA));
        }

        public double CalculatePerimeter()
        {
            return Distance(VertexA, VertexB) +
                   Distance(VertexB, VertexC) +
                   Distance(VertexC, VertexA);
        }

        public string GetTriangleType()
        {
            double a = Distance(VertexB, VertexC);
            double b = Distance(VertexA, VertexC);
            double c = Distance(VertexA, VertexB);

            if (Math.Abs(a - b) < double.Epsilon &&
                Math.Abs(b - c) < double.Epsilon)
                return "Равносторонний";

            if (Math.Abs(a - b) < double.Epsilon ||
                Math.Abs(a - c) < double.Epsilon ||
                Math.Abs(b - c) < double.Epsilon)
                return "Равнобедренный";

            return "Разносторонний";
        }

        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        public override string ToString()
        {
            return $"Треугольник [A({VertexA.X},{VertexA.Y}), B({VertexB.X},{VertexB.Y}), C({VertexC.X},{VertexC.Y})]\n" +
                   $"Тип: {GetTriangleType()}\n" +
                   $"Площадь: {CalculateArea():F2}\n" +
                   $"Периметр: {CalculatePerimeter():F2}";
        }
    }

    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";
    }
}