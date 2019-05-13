using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace RayTracer.Entities.Renderable
{
    /// <summary>
    /// MaterialFragment represents the local properties of given material like color or textures' coordinates it must be interpolable so it implements basic operators.
    /// In mathematical terms it is a member of vector space
    /// </summary>
    struct MaterialFragment
    {
        public Color Color;
        public Vector<double> TextureCoordinates;

        public MaterialFragment(Color color, Vector<double> textureCoordinates = null)
        {
            Color = color;
            TextureCoordinates = textureCoordinates ?? Utility.MakeVector(0);
        }

        public static MaterialFragment operator +(MaterialFragment first, MaterialFragment second) =>
            new MaterialFragment(
                first.Color + second.Color,
                first.TextureCoordinates + second.TextureCoordinates
            );
        public static MaterialFragment operator *(MaterialFragment fragment, double scalar) =>
            new MaterialFragment(
                scalar * fragment.Color,
                scalar * fragment.TextureCoordinates
            );
        public static MaterialFragment operator *(double scalar, MaterialFragment fragment) =>
            fragment * scalar;
        public static MaterialFragment operator /(MaterialFragment fragment, double scalar) =>
            new MaterialFragment(
                fragment.Color / scalar,
                fragment.TextureCoordinates / scalar
            );
    }
}
