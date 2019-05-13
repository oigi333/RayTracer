using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace RayTracer
{
    class Camera
    {
        private Vector<double> rotation;
        private Matrix<double> rotationMatrix;
        private double distance;
        private double fov;

        public uint Width { get; set; }
        public uint Height { get; set; }
        
        public Vector<double> Position { get; set; }

        // Besides setting rotation vector it calculates rotation matrix 
        public Vector<double> Rotation
        {
            set
            {
                rotation = value;
                
                var cosined = rotation.Select(angle => Math.Cos(angle)).ToArray();
                var sined = rotation.Select(angle => Math.Sin(angle)).ToArray();

                var rYaw = Matrix<double>.Build.DenseOfArray(new double[3, 3]
                {
                    { cosined[0], -sined[0],  0 },
                    { sined[0],   cosined[0], 0 },
                    { 0,          0,          1 }
                });
                var rPitch = Matrix<double>.Build.DenseOfArray(new double[3, 3]
                {
                    { cosined[1], 0, sined[1]   },
                    { 0,          1, 0          },
                    { -sined[1],  0, cosined[1] }
                });

                var rRoll = Matrix<double>.Build.DenseOfArray(new double[3, 3]
                {
                    { 1, 0,          0          },
                    { 0, cosined[2], sined[2]   },
                    { 0, -sined[2],  cosined[2] }
                });

                rotationMatrix = rYaw * rPitch * rRoll;
            }
            get => rotation;
        }
        public double FieldOfView
        {
            get => fov;
            set
            {
                fov = value;
                distance = Width / (2f * Math.Tan(fov / 2));
            }
        }
        public Camera(uint width, uint height, Vector<double> position, Vector<double> rotation, double fov)
        {
            Width = width;
            Height = height;
            Position = position;
            Rotation = rotation;
            FieldOfView = fov;
        }

        /// <summary>
        /// Calculates direction of ray from given point on screen.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector<double> GetDirection(int x, int y)
        {
            // The y component is negated because on screen y axis points downwards and in world space it points upwards.
            var direction = Vector<double>.Build.DenseOfArray(new double[] { x - Width / 2f,  Height / 2 - y, distance });
            return rotationMatrix*direction.Normalize(2);
        }
    }
}
