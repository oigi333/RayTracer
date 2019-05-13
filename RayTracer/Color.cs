using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer
{
    /// <summary>
    /// Class representing color which values should be between 0 and 1. 
    /// </summary>
    struct Color
    {
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }
        public static Color Zero => new Color(0, 0, 0);

        public Color(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static Color operator +(Color first, Color second) =>
            new Color(
                first.R + second.R,
                first.G + second.G,
                first.B + second.B
            );

        public static Color operator *(Color first, Color second) =>
            new Color(
                first.R * second.R,
                first.G * second.G,
                first.B * second.B
            );

        public static Color operator *(double scalar, Color color) =>
            new Color(
                scalar * color.R,
                scalar * color.G,
                scalar * color.B
            );

        public static Color operator *(Color color, double scalar) => scalar * color;
        public static Color operator /(Color color, double scalar) => (1 / scalar ) * color;
    }
}
