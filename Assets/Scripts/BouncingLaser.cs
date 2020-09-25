#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

public class BouncingLaser : MonoBehaviour
{
    [SerializeField] private int BounceCount = 5;
    [SerializeField] private float RayMaxDistance = 10.0f;

    [SerializeField] private GameObject ObjectToMoveAlongLasers = null;
    [SerializeField] private float ObjectMovementSpeed = 2.0f;

    private float ObjectMovedDistance = 0.0f;
    private List<Vector3> LaserBouncePoints = new List<Vector3>();

    private void OnDrawGizmos()
    {
        GetReflectionPoints();
        DrawBouncePoints();

#if UNITY_EDITOR
        DrawLaserLines();
#endif
    }

    private void GetReflectionPoints()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        
        List<Vector3> bouncePoints = new List<Vector3>();
        bouncePoints.Add(origin);
        
        for (int i = 0; i < BounceCount+1; i++)
        {
            if (!Physics.Raycast(origin, direction, out RaycastHit hit))
            {
                bouncePoints.Add(origin + direction * RayMaxDistance);
                break;
            }
            bouncePoints.Add(hit.point);
            
            origin = hit.point;
            direction += hit.normal;
        }

        LaserBouncePoints = bouncePoints;
    }
    
    private void DrawBouncePoints()
    {
        for (int i = 1; i < LaserBouncePoints.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(LaserBouncePoints[i], 0.03f);
            Gizmos.color = Color.white;
        }
    }
        
    private void DrawLaserLines()
    {
        Handles.color = Color.red;
        Handles.DrawAAPolyLine(LaserBouncePoints.ToArray());
        Handles.color = Color.white;
    }

    private void Update()
    {
        GetReflectionPoints();
        ObjectMovedDistance += ObjectMovementSpeed * Time.deltaTime;
        ObjectToMoveAlongLasers.transform.position = GetPointOnLaser(ObjectMovedDistance);
    }

    private Vector3 GetPointOnLaser(float pDistanceTravelled)
    {
        float distanceLeftToTravel = pDistanceTravelled;

        for (int i = 0; i < LaserBouncePoints.Count-1; i++)
        {
            float distanceToNextPoint = Vector3.Distance(LaserBouncePoints[i], LaserBouncePoints[i + 1]);
            if (distanceLeftToTravel < distanceToNextPoint)
            {
                Vector3 lastPoint = LaserBouncePoints[i];
                Vector3 nextPoint = LaserBouncePoints[i + 1];
                Vector3 directionToNextPoint = (nextPoint - lastPoint).normalized;

                return lastPoint + directionToNextPoint * distanceLeftToTravel;
            }
            distanceLeftToTravel -= distanceToNextPoint;
        }

        return LaserBouncePoints[LaserBouncePoints.Count-1];
    }
}
