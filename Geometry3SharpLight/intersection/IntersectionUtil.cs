// Modified file from https://github.com/gradientspace/geometry3Sharp

namespace Geometry3SharpLight
{
    public enum IntersectionResult
    {
        NotComputed,
        Intersects,
        NoIntersection,
        InvalidQuery
    }

    public enum IntersectionType
    {
        Empty, Point, Segment, Line, Polygon, Plane, Unknown
    }
}
