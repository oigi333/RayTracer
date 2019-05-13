using MathNet.Numerics.LinearAlgebra;

namespace RayTracer.Entities.Renderable
{
    /// <summary>
    /// Provides functionality needed to get color of the given point
    /// </summary>
    interface IColorable
    {
        Material Material { get; set; }
        MaterialFragment GetMaterialFragmentInPoint(Vector<double> point);
    }
}
