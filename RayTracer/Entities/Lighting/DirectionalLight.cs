using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace RayTracer.Entities.Lighting
{
    /// <summary>
    /// Defined Light pointing in certain direction without any origin, good example of such light type is sun.
    /// </summary>
    class DirectionalLight: ILight
    {
        public Color Color { get; set; }
        public Vector<double> Direction { get; set; }
        public DirectionalLight(Color color, Vector<double> direction)
        {
            Color = color;
            Direction = direction;
        }

        // The directions of vector from light to point and from point to light are opposite to each others, hence opposite sign  
        public (double Distance, Vector<double> Direction) GetCollisionInfo(Vector<double> point) =>
            (double.PositiveInfinity, -Direction);
    }
}
