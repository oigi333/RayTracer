using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Entities.Renderable
{
    /// <summary>
    /// Material of colorable object
    /// </summary>
    class Material
    {
        public double ReflectionCoefficient { get; set; }
        public DirectBitmap Texture { get; set; }

        public Material(double reflectionCoefficient, DirectBitmap texture = null)
        {
            ReflectionCoefficient = reflectionCoefficient;
            Texture = texture;
        }

        /// <summary>
        /// Calculates color from given fragment
        /// </summary>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public Color GetColor(MaterialFragment fragment)
        {
            // Object can be untextured
            if (Texture == null)
                return fragment.Color;
            else
            {
                var u = fragment.TextureCoordinates[0] * Texture.Width;
                var v = fragment.TextureCoordinates[1] * Texture.Height;

                return fragment.Color * Texture.GetPixel((int)u, (int)v);
            }
        }
    }
}
