using System;
using System.Collections.Generic;
using System.Linq;
using Clipper2Lib;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MathUtils
{
    public static Quaternion LookAt2D(Vector2 forward)
    {
        float angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // https://github.com/setchi/Unity-LineSegmentsIntersection/blob/master/Assets/LineSegmentIntersection/Scripts/Math2d.cs
    public static bool SegmentsIntersection(
        Vector2 oneStart, Vector2 oneEnd,
        Vector2 secondStart, Vector2 secondEnd,
        out Vector2 intersection,
        float pointsOffset = 0f)
    {
        intersection = Vector2.zero;

        float d = (oneEnd.x - oneStart.x) * (secondEnd.y - secondStart.y) - (oneEnd.y - oneStart.y) * (secondEnd.x - secondStart.x);

        if (d == 0.0f)
        {
            return false;
        }

        float u = ((secondStart.x - oneStart.x) * (secondEnd.y - secondStart.y) - (secondStart.y - oneStart.y) * (secondEnd.x - secondStart.x)) / d;
        float v = ((secondStart.x - oneStart.x) * (oneEnd.y - oneStart.y) - (secondStart.y - oneStart.y) * (oneEnd.x - oneStart.x)) / d;

        if (u < pointsOffset || u > (1f - pointsOffset)  || v < pointsOffset || v > (1f - pointsOffset))
        {
            return false;
        }

        intersection.x = oneStart.x + u * (oneEnd.x - oneStart.x);
        intersection.y = oneStart.y + u * (oneEnd.y - oneStart.y);

        return true;
    }

    public static Vector2 RandomDirection()
    {
        Vector2 result;
        result.x = Mathf.Cos(Random.value);
        result.y = Mathf.Sin(Random.value);
        return result;
    }
    
    public static Quaternion RandomRotation2D()
    {
        return Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f));
    }

    public static Vector3 ToVector3(this Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0f);
    }
    
    public static Vector2 ToVector2(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }

    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static PathD ToPathD(this Vector2[] points)
    {
        List<double> data = new List<double>();
        foreach (Vector2 point in points)
        {
            data.Add(point.x);
            data.Add(point.y);
        }

        return Clipper.MakePath(data.ToArray());
    }

    public static Vector2[] ToVectorArray(this PathD pathD)
    {
        Vector2[] result = new Vector2[pathD.Count];
        for (int pointID = 0; pointID < pathD.Count; pointID++)
        {
            result[pointID] = new Vector2((float)pathD[pointID].x, (float)pathD[pointID].y);
        }

        return result;
    }

    // https://stackoverflow.com/questions/2034540/calculating-area-of-irregular-polygon-in-c-sharp
    public static float CalculateAreaOfPolygon(Vector2[] polygon)
    {
        List<Vector2> loopedPolygon = polygon.ToList();
        loopedPolygon.Add(polygon[0]);
        
        return Math.Abs(polygon.Take(polygon.Length)
            .Select((p, i) => (loopedPolygon[i + 1].x - p.x) * (loopedPolygon[i + 1].y + p.y))
            .Sum() / 2);
    }
}