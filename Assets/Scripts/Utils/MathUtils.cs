using System.Collections.Generic;
using UnityEngine;

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
}