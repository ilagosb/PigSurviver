using UnityEngine;

public interface IAIEnemyActor
{
    Vector2 GetTarget();
    int GetPriority();

    void Move(Vector2 direction);
}