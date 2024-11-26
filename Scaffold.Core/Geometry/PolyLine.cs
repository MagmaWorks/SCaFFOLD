using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Scaffold.Core.Geometry.Abstract;
using Scaffold.Core.Geometry.Enums;

namespace Scaffold.Core.Geometry;

[ExcludeFromCodeCoverage] // because we will be using Kris' libs for this from v1.
public class PolyLine
{
    List<GeometryBase> _segments;
    public List<GeometryBase> Segments
    {
        get
        {
            return _segments.ToList();
        }
    }


    public Vector2 Start
    {
        get
        {
            return _segments.First().Start;
        }
    }

    public Vector2 End
    {
        get
        {
            return _segments.Last().End;
        }
    }

    public double Length
    {
        get
        {
            double returnLength = 0;
            foreach (var item in _segments)
            {
                returnLength += item.Length;
            }
            return returnLength;
        }
    }

    bool _isClosed = false;
    public bool IsClosed
    {
        get
        {
            return _isClosed;
        }
        set
        {
            if (value)
            {
                if ((End - Start).Length() < 0.1)
                {
                    _isClosed = value;
                }
                else
                {
                    _segments.Add(new Line(End, Start));
                }
            }
            else
                _isClosed = value;
        }
    }

    public PolyLine(List<GeometryBase> segments)
    {
        _segments = segments;
    }

    public List<IntersectionResult> intersection(Line line) // need to add full support for NONE and PROJECTED 
    {
        var totalLength = Length;
        double lengthToSegment = 0;
        var returnList = new List<IntersectionResult>();
        foreach (var segment in _segments)
        {
            var intersections = segment.intersection(line);
            foreach (var intersection in intersections)
            {
                if (intersection.TypeOfIntersection == IntersectionType.WITHIN)
                {
                    returnList.Add(new IntersectionResult((lengthToSegment + intersection.Parameter * segment.Length) / totalLength, intersection.Point, intersection.TypeOfIntersection));
                }
            }
            lengthToSegment += segment.Length;
        }
        if (returnList.Count == 0)
        {
            returnList.Add(new IntersectionResult(double.NaN, new Vector2(), IntersectionType.NONE));
        }
        return returnList;
    }

    public override string ToString()
    {
        var returnString = "";
        returnString = "Start " + _segments[0].Start.ToString();
        returnString += Environment.NewLine;

        foreach (var item in _segments)
        {
            if (item.GetType() == typeof(Arc))
            {
                returnString += " Arc ";
                returnString += item.End;
                returnString += Environment.NewLine;
            }
            if (item.GetType() == typeof(Line))
            {
                returnString += " Line ";
                returnString += item.End;
                returnString += Environment.NewLine;
            }
        }

        return returnString;
    }

    public Vector2 PointAtParameter(double parameter)
    {
        var totalLength = Length;
        double lengthToSegmentStart = 0;
        double lengthToSegmentEnd = 0;
        for (var i = 0; i < _segments.Count; i++)
        {
            var segment = _segments[i];
            lengthToSegmentStart = lengthToSegmentEnd;
            lengthToSegmentEnd += segment.Length;
            var parameterToSegmentEnd = lengthToSegmentEnd / totalLength;
            var parameterToSegmentStart = lengthToSegmentStart / totalLength;
            if (parameter >= parameterToSegmentStart && parameter <= parameterToSegmentEnd)
            {
                var segmentParameter = (parameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                if (double.IsNaN(segmentParameter))
                    segmentParameter = 0;
                return segment.PointAtParameter(segmentParameter);
            }
        }
        return _segments.Last().PointAtParameter(1);
    }

    public Vector2 PerpAtParameter(double parameter)
    {
        var totalLength = Length;
        double lengthToSegmentStart = 0;
        double lengthToSegmentEnd = 0;
        for (var i = 0; i < _segments.Count; i++)
        {
            var segment = _segments[i];
            lengthToSegmentStart = lengthToSegmentEnd;
            lengthToSegmentEnd += segment.Length;
            var parameterToSegmentEnd = lengthToSegmentEnd / totalLength;
            var parameterToSegmentStart = lengthToSegmentStart / totalLength;
            if (parameter >= parameterToSegmentStart && parameter <= parameterToSegmentEnd)
            {
                var segmentParameter = (parameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                if (double.IsNaN(segmentParameter))
                    segmentParameter = 0;
                return segment.PerpAtParameter(segmentParameter);
            }
        }
        return _segments.Last().PerpAtParameter(1);
    }

    public PolyLine Cut(double startParameter, double endParameter)
    {
        var totalLength = Length;
        double lengthToSegmentStart = 0;
        double lengthToSegmentEnd = 0;
        var started = false;
        var finished = false;
        var segments = new List<GeometryBase>();
        for (var i = 0; i < _segments.Count; i++)
        {
            var segment = _segments[i];
            lengthToSegmentStart = lengthToSegmentEnd;
            lengthToSegmentEnd += segment.Length;
            var parameterToSegmentEnd = lengthToSegmentEnd / totalLength;
            var parameterToSegmentStart = lengthToSegmentStart / totalLength;
            if ((endParameter > parameterToSegmentEnd && started) || (endParameter <= startParameter && started))
            {
                segments.Add(segment);
            }
            else if (startParameter <= parameterToSegmentEnd && !started)
            {
                if (endParameter == 0.1)
                {
                    //
                }
                if (endParameter <= parameterToSegmentEnd && endParameter >= startParameter)
                {
                    var segmentParameter = (endParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                    if (double.IsNaN(segmentParameter))
                        segmentParameter = 0;
                    var partialCut = segment.Cut(segmentParameter)[0];
                    var secondSegmentParameter = (startParameter - parameterToSegmentStart) / (endParameter - parameterToSegmentStart);
                    segments.Add(partialCut.Cut(secondSegmentParameter)[1]);
                    started = true;
                    finished = true;
                }
                else
                {
                    var segmentParameter = (startParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                    if (double.IsNaN(segmentParameter))
                        segmentParameter = 0;
                    segments.Add(segment.Cut(segmentParameter)[1]);
                    started = true;
                }
            }
            else if (endParameter <= parameterToSegmentEnd && endParameter >= startParameter && started && !finished)
            {
                var segmentParameter = (endParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                if (double.IsNaN(segmentParameter))
                    segmentParameter = 0;
                segments.Add(segment.Cut(segmentParameter)[0]);
                finished = true;
                break;
            }
        }
        if (finished)
        {
            return new PolyLine(segments);
        }
        lengthToSegmentEnd = 0;
        lengthToSegmentStart = 0;
        for (var i = 0; i < _segments.Count; i++)
        {
            var segment = _segments[i];
            lengthToSegmentStart = lengthToSegmentEnd;
            lengthToSegmentEnd += segment.Length;
            var parameterToSegmentEnd = lengthToSegmentEnd / totalLength;
            var parameterToSegmentStart = lengthToSegmentStart / totalLength;
            if (endParameter > parameterToSegmentEnd)
            {
                segments.Add(segment);
            }
            //else if (startParameter <= parameterToSegmentEnd && !started)
            //{
            //    double segmentParameter = (startParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
            //    segments.Add(segment.Cut(segmentParameter)[1]);
            //    started = true;
            //}
            else if (endParameter <= parameterToSegmentEnd)
            {
                var segmentParameter = (endParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                if (double.IsNaN(segmentParameter))
                    segmentParameter = 0;
                segments.Add(segment.Cut(segmentParameter)[0]);
                finished = true;
                break;
            }
        }
        return new PolyLine(segments);
    }
}
