
using UnityEngine.Events;

public interface IDamagable
{
    event UnityAction<int> Damaged;
    void ToDamage(int damage);
}
