
using UnityEngine;

public class LifeEntityProvider : ITargetsProvider
{
    private ALifeEntity _entity;


    public LifeEntityProvider(ALifeEntity entity)
    {
        _entity = entity;
    }

    public Vector2 ProvideTarget()
    {
        return _entity.transform.position;
    }

    public int ProvidePriority()
    {
        return 3;
    }
}
