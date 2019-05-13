using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracer.Entities.Renderable
{
    /// <summary>
    /// In order for object to be renderable it must be collidable and colorable too.
    /// </summary>
    interface IRenderable: ICollidable, IColorable
    {
    }
}
