using UnityEngine;

public class Farmer : Enemy
{
    public override void SetProvider(ITargetsProvider provider)
    {
        var resultProvider = new PatrolFromRandomPoint(provider, 2);
        base.SetProvider(resultProvider);
    }

    public class PatrolFromRandomPoint : ITargetsProvider
    {
        private ITargetsProvider _childProvider;

        private Vector2 _patrolTarget = Vector2.negativeInfinity;

        private float _timePatrol;
        private float _timePatrolNecessary;
        
        public PatrolFromRandomPoint(ITargetsProvider childProvider, float timePatrolNecessary)
        {
            _childProvider = childProvider;
            _timePatrolNecessary = timePatrolNecessary;
            _timePatrol = timePatrolNecessary;
        }
        
        

        private Vector2 GetRandomTarget()
        {
            var points = GameModel.Instance.GameArea.GetAreaPoints();
            var randomPoint = points[Random.Range(0, points.Count - 1)];
            return new Vector2(randomPoint.X, randomPoint.Y);
        }

        public Vector2 ProvideTarget()
        {
            var childTarget = _childProvider.ProvideTarget();
            var priorityChild = _childProvider.ProvidePriority();
            var priority = ProvidePriority();
            if (priorityChild <= priority)
            {
                if (_timePatrol >= _timePatrolNecessary || _patrolTarget == Vector2.negativeInfinity)
                {
                    _patrolTarget = GetRandomTarget();
                    _timePatrol = 0;
                }
                _timePatrol += Time.deltaTime;
                return _patrolTarget;
            }

            _timePatrol = 0;
            return childTarget;
        }

        public int ProvidePriority()
        {
            return 1;
        }
    }
}
