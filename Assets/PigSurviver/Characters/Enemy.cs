using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Animator))]
public abstract class Enemy : MonoBehaviour, IDamagable, IAIEnemyActor
{
    protected int _damage = 1;

    [SerializeField]
    protected float _distanceAggro = .5f;
    
    public float DistanceAggro => _distanceAggro;
    
    [SerializeField]
    protected float _speed = 2;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    
    [SerializeField] 
    protected float _argueSpeedIncrease;

    [SerializeField] 
    protected float _dirtySpeedDecrease = 1f;
    
    public float ArgueSpeedIncrease => _argueSpeedIncrease;


    private AudioSource _audio;
    
    public AudioSource Audio => _audio;

    [SerializeField] 
    private AudioClip _angrySound;

    public AudioClip AngrySound => _angrySound;
    
    [SerializeField] 
    private AudioClip _idleSound;

    public AudioClip IdleSound => _idleSound;

    [SerializeField]
    private GameObject _ash;

    public GameObject Ash => _ash;
    
    private Animator _anim;
    public static readonly int VelocityY = Animator.StringToHash("VelocityY");
    public static readonly int VelocityX = Animator.StringToHash("VelocityX");
    public static readonly int Dirty = Animator.StringToHash("IsDirty");
    private ITargetsProvider _provider = new InvalidProvider();
    
    private bool _isDirty;
    

    public bool IsDirty
    {
        get => _isDirty;
        set
        {
            _isDirty = value;
            if (_isDirty)
            {
                EventGotDirty();
            }
        }
    }

    private event UnityAction EventGotDirty;
    public event UnityAction GotDirty
    {
        add => EventGotDirty += value;
        remove => EventGotDirty -= value;
    }

    private event UnityAction EventDied;
    public event UnityAction Died
    {
        add => EventDied += value;
        remove => EventDied -= value;
    }
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        EventGotDirty += OnGotDirty;
    }

    private void OnGotDirty()
    {
        float trueSpeed = _speed;
        _speed -= _dirtySpeedDecrease;
        StartCoroutine(DisableDirtyDelay(trueSpeed));
    }

    private IEnumerator DisableDirtyDelay(float speed)
    {
        yield return new WaitForSecondsRealtime(4);
        _anim.SetBool(Dirty, false);
        _speed = speed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Pig pig))
        {
            pig.ToDamage(_damage);
        }
    }
    
    public virtual void SetProvider(ITargetsProvider provider)
    {
        Debug.Log(provider + " setting");
        _provider = provider;
    }

    private event UnityAction<int> EventDamaged; 
    public event UnityAction<int> Damaged
    {
        add => EventDamaged += value;
        remove => EventDamaged -= value;
    }

    public void ToDamage(int damage)
    {
        EventDamaged(damage);
        Instantiate(_ash, transform.position, Quaternion.identity);
        Destroy(gameObject, .5f);
    }

    public Vector2 GetTarget()
    {
        return _provider.ProvideTarget();
    }
    

    public int GetPriority()
    {
        return _provider.ProvidePriority();
    }

    public void Move(Vector2 target)
    {
        Vector2 difference = new Vector3(target.x, target.y) - transform.position;
        Vector2 direction = difference.x > 0 ? Vector2.right :
            difference.x < 0 ? Vector2.left :
            difference.y > 0 ? Vector2.up : Vector2.down;
        _anim.SetFloat(VelocityX, direction.x);
        _anim.SetFloat(VelocityY, direction.y);
        transform.position = Vector2.MoveTowards(transform.position, target,
            Time.deltaTime * _speed);
    }
    

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _distanceAggro);
    }
#endif

    public class InvalidProvider : ITargetsProvider
    {
        public Vector2 ProvideTarget()
        {
            return Vector2.negativeInfinity;
        }

        public int ProvidePriority()
        {
            return -1;
        }
    }
    
    public class ArgueZoneProvider : ITargetsProvider
    {
        private float _zoneDistance;

        private Transform _zone;

        private int _mask;

        private bool isCachedTarget = true;
        private Transform _cacheTarget = null;

        private int _priority = 1;

        public ArgueZoneProvider(float zoneDistance, Transform zone, int mask)
        {
            _zone = zone;
            _zoneDistance = zoneDistance;
            _mask = mask;
        }
        
        public Vector2 ProvideTarget()
        {
            if (_cacheTarget != null)
                return _cacheTarget.position;
            Collider2D overlapCircle = Physics2D.OverlapCircle(_zone.position, _zoneDistance, _mask);
            if (overlapCircle != null && overlapCircle.TryGetComponent(out Pig pig))
            {
                _priority = 2;
                if (isCachedTarget)
                    _cacheTarget = pig.transform;
                return pig.transform.position;
            }
            _priority = -1;
            return Vector2.negativeInfinity;
        }

        public int ProvidePriority()
        {
            return _priority;
        }
    }

    
}
