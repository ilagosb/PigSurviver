using UnityEngine;

public interface ITargetsProvider
{
    Vector2 ProvideTarget();
    int ProvidePriority();
}
