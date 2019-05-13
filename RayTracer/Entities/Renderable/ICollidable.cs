using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace RayTracer.Entities.Renderable
{
    /// <summary>
    /// Provides functionality needed to check if object collide with ray.
    /// </summary>
    interface ICollidable
    {
        Vector<double> GetNormal(Vector<double> point);
        double? GetCollisionDistance(Vector<double> origin, Vector<double> direction);
    }
}
