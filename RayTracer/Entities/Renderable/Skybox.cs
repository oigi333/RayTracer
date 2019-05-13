using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace RayTracer.Entities.Renderable
{
    /// <summary>
    /// Represents Skybox, because the supposed terrain is far away the origin of the ray is irrelevant.
    /// </summary>
    class Skybox
    {
        public DirectBitmap Texture {get;set;}

        public Skybox(string filename)
        {
            Texture = new DirectBitmap(filename);
        }


        public Color GetColorInDirection(Vector<double> direction)
        {
            // It is basically conversion between cartesian and spherical coordinates. 
            var phi = Math.Acos(direction[1]) / (Math.PI);
            var theta = (Math.Atan2(direction[2], direction[0]) + Math.PI) / (2 * Math.PI);

            return Texture.GetPixel((int)(theta * Texture.Width), (int)(phi * Texture.Height));
        }
    }
}
