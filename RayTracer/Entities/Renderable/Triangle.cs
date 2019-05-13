using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

using System.Drawing;

namespace RayTracer.Entities.Renderable
{
    class Triangle : IRenderable
    {
        Vector<double>[] verticiesPositions;
        /// <summary>
        /// Vericies position mapped on the plane
        /// </summary>
        Vector<double>[] verticiesPositionsOnPlane;
        /// <summary>
        /// Normal to the plane.
        /// </summary>
        Vector<double> planeNormal;
        /// <summary>
        /// Mapping matrix
        /// </summary>
        /// <example>mapping*vectorInWorldCoordinatesLayingOnPlane = vectorInPlaneCoordinates</example>
        Matrix<double> mapping;

        public Vector<double>[] VerticiesPositions {
            get => verticiesPositions;
            set
            {
                verticiesPositions = value;
                
                // First axis of plane's coordinates is an edge of triangle
                var i = (verticiesPositions[1] - verticiesPositions[0]).Normalize(2);
                // Second axis is normal of the plane
                planeNormal = -Utility.Cross(
                    verticiesPositions[0] - verticiesPositions[1], verticiesPositions[2] - verticiesPositions[0]
                ).Normalize(2);
                // Third axis is vector perpendicular to both edge and normal
                var j = Utility.Cross(i, planeNormal);
                // Matrix created from this three axis map plane coordinates to world coordinates and we need to inverse it
                mapping = Matrix<double>.Build.DenseOfColumnVectors(i, j, planeNormal).Inverse();
                
                verticiesPositionsOnPlane = new Vector<double>[]
                {
                    Utility.MakeVector(new double[]{ 0, 0, 0 }),
                    mapping * (verticiesPositions[1] - verticiesPositions[0]),
                    mapping * (verticiesPositions[2] - verticiesPositions[0])
                };

            }
        }
        public MaterialFragment[] VerticiesFragments { get; set; }
        public Material Material { get; set; }

        public Triangle(Vector<double>[] verticiesPositions, MaterialFragment[] verticiesFragments, Material material)
        {
            VerticiesPositions = verticiesPositions;
            VerticiesFragments = verticiesFragments;
            Material = material;
        }

        public double? GetCollisionDistance(Vector<double> origin, Vector<double> direction)
        { 
            var directionNormalComponent = direction * mapping.Row(2);
            if (Math.Abs(directionNormalComponent) < double.Epsilon)
                return null;

            // mapping * (origin + distance * direction - verticiesPosition[0]) = [ x, y, 0 ]
            double distance = -((origin - verticiesPositions[0]) * mapping.Row(2)) / directionNormalComponent;
            if (distance <= 0)
                return null;

            var point = origin + distance * direction;
            var mappedPoint = mapping * (point - verticiesPositions[0]);
            // Mapped point can be converted to barycentric coordinates
            var barycentric = ToBarycentric(mappedPoint);

            // In barycentric coordinates every component is nonnegative
            return barycentric[0] >= 0 && barycentric[1] >= 0 && barycentric[2] >= 0 ? distance : (double?)null;
        }

        public MaterialFragment GetMaterialFragmentInPoint(Vector<double> point)
        {
            var mappedPoint = mapping * (point - verticiesPositions[0]);
            var wages = ToBarycentric(mappedPoint);

            // Weighted arithmetic mean of fragments.
            var fragment = wages[0] * VerticiesFragments[0] + wages[1] * VerticiesFragments[1] + wages[2] * VerticiesFragments[2];
            fragment /= wages[0] + wages[1] + wages[2];

            return fragment;
        }

        // This is to be changed, because this normal is used in light calculation and all veriticies can have their own normal.
        public Vector<double> GetNormal(Vector<double> point) => planeNormal;

        private Vector<double> ToBarycentric(Vector<double> cartesian)
        {
            var v = verticiesPositionsOnPlane;
            var denominator = (v[1][1] - v[2][1]) * v[2][0] + (v[2][0] - v[1][0]) * v[2][1];

            //magic formula
            var W1 = -((v[1][1] - v[2][1]) * (cartesian[0] - v[2][0]) + (v[2][0] - v[1][0]) * (cartesian[1] - v[2][1])) / denominator;
            var W2 = -(v[2][1] * (cartesian[0] - v[2][0]) - v[2][0] * (cartesian[1] - v[2][1])) / denominator;
            var W3 = 1 - W1 - W2;
            
            return Utility.MakeVector(new double[]
            {
                W1,
                W2,
                W3

            });
        }

        
    }
}
