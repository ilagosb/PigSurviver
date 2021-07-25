
using UnityEngine;
using UnityEngine.Events;

public interface ILife
{
    // Called every time when was changed life with oldValue and newValue
    event UnityAction<int, int> LifeCountChanged; 
    int GetLife();
}

public abstract class ALifeEntity : MonoBehaviour, ILife
{
    private int _life;
    protected int Life
    {
        set
        {
            int oldValue = _life;
            _life = value;
            EventLifeCountChanged(oldValue, _life);
        }
        get => _life;
    }

    private event UnityAction<int, int> EventLifeCountChanged;
    public event UnityAction<int, int> LifeCountChanged
    {
        add => EventLifeCountChanged += value;
        remove => EventLifeCountChanged += value;
    }

    public int GetLife()
    {
        return Life;
    }
}
