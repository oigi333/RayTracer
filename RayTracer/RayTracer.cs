using System;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;
using System.Threading.Tasks;
using ShellProgressBar;
using RayTracer.Entities;
using RayTracer.Entities.Renderable;
using RayTracer.Entities.Lighting;

namespace RayTracer
{
    class RayTracer
    {
        public Camera Camera { get; set; }
        public DirectBitmap Bitmap { get; set; }
        public Skybox Skybox { get; set; }
        public IRenderable[] Scene { get; set; }
        public ILight[] Lights { get; set; }
        /// <summary>
        /// Max depth represent number of possible recursion levels.
        /// </summary>
        public uint MaxDepth { get; set; }

        public RayTracer(Camera camera, DirectBitmap bitmap, IRenderable[] scene, ILight[] lights)
        {
            Camera = camera;
            Bitmap = bitmap;
            Scene = scene;
            Lights = lights;
            MaxDepth = 4;

            Skybox = new Skybox("res/skybox.jpg");
        }
        

        public void RayTrace()
        {
            using (var bar = new ProgressBar((int)(Camera.Height * Camera.Width), "Raytracing", ConsoleColor.Cyan))
            {
                // Simple parallelization
                Parallel.For(0, Camera.Height * Camera.Width, i =>
                {
                    int x = (int)(i % Camera.Width);
                    int y = (int)(i / Camera.Width);

                    var pixelColor = TraceSingleRay(Camera.Position, Camera.GetDirection(x, y));

                    Bitmap.SetPixel(x, y, pixelColor);
                    bar.Tick();
                });
            }
        }

        /// <summary>
        /// Trace single ray in a given direction.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        /// <param name="depth">Current recursion level.</param>
        /// <param name="lastReflected">The object from ray was cast.</param>
        /// <returns></returns>
        Color TraceSingleRay(Vector<double> origin, Vector<double> direction, uint depth = 0, ICollidable lastReflected = null)
        {
            if (depth >= MaxDepth)
                return Skybox.GetColorInDirection(direction);

            var nearest = GetNearestEntity(origin, direction, lastReflected);

            // If nearest entity is null it means that ray hasn't collide with anything
            if (nearest.Entity == null)
                return Skybox.GetColorInDirection(direction) * new Color(0.5, 0.5, 0.5);

            // Point of collision.
            var point = origin + nearest.Distance * direction;

            var entity = nearest.Entity;

            var material = entity.Material;
            var fragment = entity.GetMaterialFragmentInPoint(point);

            var localColor = material.GetColor(fragment);
            var diffuseColor = GetDiffuseColor(entity, point);

            // If reflection coefficient is zero no further casting is needed
            var reflectionColor = Color.Zero;
            if (material.ReflectionCoefficient > double.Epsilon)
                reflectionColor = GetReflectionColor(entity, point, direction, depth);


            return localColor * ( diffuseColor + material.ReflectionCoefficient * reflectionColor);
        }

        (IRenderable Entity, double Distance) GetNearestEntity(Vector<double> origin, Vector<double> direction, ICollidable lastReflected = null)
        {
            (IRenderable Entity, double Distance) nearest = (null, double.PositiveInfinity);

            foreach (var entity in Scene)
            {
                // If this ray was casted from this entity continue
                if (entity == lastReflected)
                    continue;

                var distance = entity.GetCollisionDistance(origin, direction);


                // if ray hasn't hit continue to next object
                if (distance == null)
                    continue;
                // if object is nearer then the current nearest it becomes new nearest 
                if (nearest.Distance > distance)
                    nearest = (entity, (double)distance);
            }

            return nearest;
        }

        Color GetDiffuseColor(ICollidable collidable, Vector<double> point)
        {
            var diffuseColor = Color.Zero;
            var normal = collidable.GetNormal(point);
            
            foreach (var light in Lights)
            {
                var lightInfo = light.GetCollisionInfo(point);

                // Check if point is occluded
                bool hasHit = false;
                foreach (var entity in Scene)
                {
                    if (entity == collidable)
                        continue;
                    var distance = entity.GetCollisionDistance(point, lightInfo.Direction);

                    if (distance != null)
                    {
                        hasHit = true;
                        break;
                    }
                }

                // If is not, add color to diffuse color
                if (!hasHit)
                {
                    diffuseColor += light.Color * Math.Max(0, (normal * lightInfo.Direction));
                }
            }

            // Minimal light intensivity
            diffuseColor += new Color(0.15, 0.15, 0.15);
            return diffuseColor;
        }
        Color GetReflectionColor(ICollidable collidable, Vector<double> point, Vector<double> direction, uint depth)
        {
            // Reflection is fairly simple just cast the ray accordingly to the first law of reflection.
            var normal = collidable.GetNormal(point);
            var newDirection = direction - 2*(normal * direction) * normal;

            return TraceSingleRay(point, newDirection, depth + 1, collidable);
        }
    }
}
