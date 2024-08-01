using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace raytracer
{
    class SceneLights
    {
        public List<PointLight> sceneLights = new List<PointLight>();
        public SceneLights()
        {
            sceneLights.Add(new PointLight(new Vector3(3f, 2f, -2f), new Vector3(1f, 1f, 1f), 1f, 5f));
            sceneLights.Add(new PointLight(new Vector3(1f, 3f, 4f), new Vector3(1f, 1f, 1f), 1f, 6f));
        }
    }
}