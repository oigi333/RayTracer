using System;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;

namespace RayTracer.Entities.Renderable
{
    class Sphere : IRenderable
    {
        public Vector<double> Center { get; set; }
        public double Radius { get; set; }
        public Material Material { get; set; }

        public Sphere(Vector<double> center, double radius, Material material)
        {
            Center = center;
            Radius = radius;
            Material = material;
        }

        public double? GetCollisionDistance(Vector<double> origin, Vector<double> direction)
        {
            // ||origin + distance*direction - center|| = radius.
            var r = origin - Center;
            // Solve quadratic equation.
            // distance^2*||direction||^2 + distance(2*direction*r) + ||r||^2 - radius^2 = 0.
            // Direction is normalized so a = 1.
            var b = 2 * r * direction;
            var c = r * r - Radius * Radius;

            var delta = b * b - 4 * c;

            // If the delta is less or equal 0 ray missed or is tangent to sphere.
            if (delta <= 0)
                return null;

            var smallerDistance = (-b - Math.Sqrt(delta)) / 2;
            // If smallerDistance is smaller than 0, the extension of ray hits the sphere but not the ray itself.
            return smallerDistance > 0 ? smallerDistance : (double?)null;
        }

        public MaterialFragment GetMaterialFragmentInPoint(Vector<double> point) => new MaterialFragment(new Color(1, 1, 1));

        public Vector<double> GetNormal(Vector<double> point) => (point - Center).Normalize(2);
    }
}
