using System;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using RayTracer.Entities.Renderable;
using RayTracer.Entities.Lighting;

namespace RayTracer
{
    class Program
    {
        static readonly uint height = 720, width = (uint)(16f * height / 9f);

        /// <summary>
        /// Creates a basic scene, later on I want to create some loader.
        /// </summary>
        /// <returns></returns>
        static IRenderable[] GetScene()
        {
            var scene = new IRenderable[]{
                //Sphere(Utility.MakeVector(200, 200, 854 / 2 + 150), 100),
                new Sphere(Utility.MakeVector(-200, 200, 854 / 2 + 150), 100, new Material(1)),
                new Sphere(Utility.MakeVector(200, -200, 854 / 2 + 150), 100, new Material(1)),
                new Sphere(Utility.MakeVector(-200, -200, 854 / 2 + 150), 100, new Material(1)),
                new Sphere(Utility.MakeVector(200, 200, 854 / 2 + 150), 100, new Material(1)),
                new Triangle(new[]
                {
                    Utility.MakeVector(-(853 / 2 + 350), (853 / 2 + 350)*9/16, +853 / 2 + 350),
                    Utility.MakeVector((853 / 2 + 350), (853 / 2 + 350)*9/16, +853 / 2 + 350),
                    Utility.MakeVector(-(853 / 2 + 350), -(853 / 2 + 350)*9/16, +853 / 2 + 350)
                },
                new[]
                {
                    new MaterialFragment(
                        new Color(1, 1, 1),
                        Utility.MakeVector(0, 0)
                    ),
                    new MaterialFragment(
                        new Color(1, 1, 1),
                        Utility.MakeVector(1, 0)
                    ),
                    new MaterialFragment(
                        new Color(1, 1, 1),
                        Utility.MakeVector(0, 1)
                   )
                },
                new Material(0.5, new DirectBitmap("res/metal.jpg"))
                ),
                new Triangle(new[]
                {
                    Utility.MakeVector((853 / 2 + 350), -(853 / 2 + 350)*9/16, +853 / 2 + 350),
                    Utility.MakeVector(-(853 / 2 + 350), -(853 / 2 + 350)*9/16, +853 / 2 + 350),
                    Utility.MakeVector((853 / 2 + 350), (853 / 2 + 350)*9/16, +853 / 2 + 350),

                },
                new[]
                {
                    new MaterialFragment(
                        new Color(1, 1, 1),
                        Utility.MakeVector(1, 1)
                    ),
                    new MaterialFragment(
                        new Color(1, 1, 1),
                        Utility.MakeVector(0, 1)
                    ),
                    new MaterialFragment(
                        new Color(1, 1, 1),
                        Utility.MakeVector(1, 0)
                   )
                },
                new Material(0.5, new DirectBitmap("res/metal.jpg"))
                ),
            }; 

            return scene;
        }

        static ILight[] GetLights()
        {
            return new[]
            {
                new DirectionalLight(new Color(0.5, 0.5, 0.5), Utility.MakeVector(0, -Math.Sqrt(2)/2, Math.Sqrt(2)/2)),
                //new DirectionalLight(new Color(1, 1, 1), Utility.MakeVector(-Math.Sqrt(2)/2, -Math.Sqrt(2)/2, 0))
            };
        }

        static void Main(string[] args)
        {
            var camera = new Camera(width, height, Utility.MakeVector(new double[] { 0, 0, 0 }), Utility.MakeVector(new double[] { 0, 0, 0 }), Math.PI / 2);
            var bitmap = new DirectBitmap((int)width, (int)height);


            var scene = GetScene();
            var lights = GetLights();

            var rayTracer = new RayTracer(camera, bitmap, scene, lights);

            rayTracer.RayTrace();

            bitmap.UnlockBits();
            bitmap.Save("output.bmp");
        }
    }
}
