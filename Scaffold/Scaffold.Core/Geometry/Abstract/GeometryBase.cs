﻿using System.Numerics;

namespace Scaffold.Core.Geometry.Abstract
{
    public abstract class GeometryBase
    {
        public abstract Vector2 Start { get; }
        public abstract Vector2 End { get; }
        public abstract double Length { get; }
        public abstract List<IntersectionResult> intersection(Line line);
        public abstract List<GeometryBase> Cut(double parameter);
        public abstract Vector2 PointAtParameter(double parameter);
        public abstract Vector2 PerpAtParameter(double parameter);
    }
}