using UnityEngine;
using static Farmer;

public class Dog : Enemy
{
    public override void SetProvider(ITargetsProvider provider)
    {
        var resultProvider = new PatrolFromRandomPoint(provider, 2);
        base.SetProvider(resultProvider);
    }
}
