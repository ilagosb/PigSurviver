using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameArea;

public class AIMovement : MonoBehaviour
{
    private Pathfinder _pathfinder;

    
    private IAIEnemyActor _actor;

    private int _lastPriority = - 1;
    private Vector2 _lastTarget = Vector2.negativeInfinity;

    private Coroutine _goingByPath;

#if UNITY_EDITOR
    private List<AreaPoint> _debugPath = new List<AreaPoint>();
#endif

    private void Awake()
    {
        _actor = GetComponent<IAIEnemyActor>();
        _pathfinder = GameModel.Instance.Pathfinder;
    }

    private void Update()
    {
        Vector2 target = _actor.GetTarget();
        int targetPriority = _actor.GetPriority();
        if (targetPriority > -1  && target != _lastTarget && targetPriority >= _lastPriority)
        {
            List<AreaPoint> path = _pathfinder.FindPath(transform.position, target);
           if (path != null)
           {
#if UNITY_EDITOR
               _debugPath = path;
#endif
               if (_goingByPath != null)
               {
                   StopCoroutine(_goingByPath);
               }

               _goingByPath = StartCoroutine(GoByPath(path));
           }
        }
        
        _lastTarget = target;
        _lastPriority = targetPriority;
    }

    public IEnumerator GoByPath(List<AreaPoint> list)
    {
        var currentIndex = 0;
        AreaPoint currentPoint = list[currentIndex];
        while (true)
        {
            if (transform.position == new Vector3(currentPoint.X, currentPoint.Y))
            {
                currentIndex++;
                if (currentIndex >= list.Count)
                {
                    yield break;
                }
                currentPoint = list[currentIndex];
            }
            _actor.Move(new Vector2(currentPoint.X, currentPoint.Y));
            yield return null;
        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_debugPath != null && _debugPath.Count > 0)
        {
            Gizmos.color = Color.black;
            AreaPoint prefPoint = null;
            foreach(var point in _debugPath)
            {
                Gizmos.DrawCube(new Vector3(point.X, point.Y), new Vector3(.09f, .09f));
                if (prefPoint != null)
                {
                    Gizmos.DrawLine(new Vector3(point.X, point.Y), new Vector3(prefPoint.X, prefPoint.Y));
                }
                prefPoint = point;
            }
        }
        
    }
#endif
}