using UnityEngine;
using UnityEngine.U2D;

public static class ObjectUtils
{
   public static Vector3[] GetPoints(this Spline spline)
   {
      Vector3[] outPoints = new Vector3[spline.GetPointCount()];
      for (int pointID = 0; pointID < spline.GetPointCount(); pointID++)
      {
         outPoints[pointID] = spline.GetPosition(pointID);
      }

      return outPoints;
   }
   
   public static Vector2[] GetPoints2D(this Spline spline)
   {
      Vector2[] outPoints = new Vector2[spline.GetPointCount()];
      for (int pointID = 0; pointID < spline.GetPointCount(); pointID++)
      {
         outPoints[pointID] = spline.GetPosition(pointID).ToVector2();
      }

      return outPoints;
   }
}