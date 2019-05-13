using MathNet.Numerics.LinearAlgebra;

namespace RayTracer.Entities.Lighting
{
    /// <summary>
    /// Defines the functionality needed for object to be a light
    /// </summary>
    interface ILight
    {
        Color Color { get; set; }

        // I'll probably create a class in place of tuple, but as long as I need only this two fields, it'll suffice
        (double Distance,  Vector<double> Direction) GetCollisionInfo(Vector<double> point);
    }
}
