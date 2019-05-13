using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace RayTracer
{
    static class Utility
    {
        public static Vector<double> MakeVector(params double[] array) => Vector<double>.Build.DenseOfArray(array);
        public static Vector<double> Cross(Vector<double> a, Vector<double> b) => MakeVector(new double[]
        { 
            a[1] * b[2] - a[2] * b[1],
            a[2] * b[0] - a[0] * b[2],
            a[0] * b[1] - a[1] * b[0],
        });


    }
}
