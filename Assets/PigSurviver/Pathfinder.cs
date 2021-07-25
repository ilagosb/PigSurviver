using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using static GameArea;

[RequireComponent(typeof(GameArea))]
public class Pathfinder : MonoBehaviour
{
    private IProviderAreaPoints _areaPoints;

    private void Awake()
    {
        _areaPoints = GetComponent<IProviderAreaPoints>();
    }

    [CanBeNull]
    public List<AreaPoint> FindPath(Vector2 start, Vector2 targetPoint)
    {
        var startPoint = _areaPoints.AreaPointFromWorldPoint(start);
        var endPoint = _areaPoints.AreaPointFromWorldPoint(targetPoint);
        
        List<AreaPoint> openPoints = new List<AreaPoint>();
        HashSet<AreaPoint> closedPoints = new HashSet<AreaPoint>();
        
        openPoints.Add(startPoint);
        
        while (openPoints.Count > 0)
        {
            var lowCostPoint = openPoints[0];
            for (int i = 1; i < openPoints.Count; i++)
            {
                if (openPoints[i].FCost < lowCostPoint.FCost || openPoints[i].FCost == lowCostPoint.FCost &&
                    openPoints[i].HCost < lowCostPoint.HCost)
                {
                    lowCostPoint = openPoints[i];
                }
            }

            openPoints.Remove(lowCostPoint);
            closedPoints.Add(lowCostPoint);

            if (lowCostPoint.Equals(endPoint))
            {
                return GetPath(startPoint, endPoint);
            }

            var closestPoints = _areaPoints.ClosestAreaPoints(lowCostPoint);
            foreach (var closest in closestPoints)
            {
                if (closest.IsClose || closedPoints.Contains(closest))
                {
                    continue;
                }

                int newCostMovement = lowCostPoint.GCost + GetDistance(lowCostPoint, closest);
                if(newCostMovement < closest.GCost || !openPoints.Contains(closest))
                {
                    closest.GCost = newCostMovement;
                    closest.HCost = GetDistance(closest, endPoint);
                    closest.prev = lowCostPoint;
                    if (!openPoints.Contains(closest))
                    {
                        openPoints.Add(closest);
                    }
                }
            }
        }

        return null;
    }

    private int GetDistance(AreaPoint from, AreaPoint to)
    {
        int distanceX = Mathf.Abs(from.GridX - to.GridX);
        int distanceY = Mathf.Abs(from.GridY - to.GridY);
        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }

    private List<AreaPoint> GetPath(AreaPoint start, AreaPoint targetPoint)
    {
        List<AreaPoint> path = new List<AreaPoint>();
        AreaPoint currentPoint = targetPoint;
        
        while (currentPoint != start)
        {
            path.Add(currentPoint);
            currentPoint = currentPoint.prev;
        }

        path.Reverse();
        return path;
    }
}
