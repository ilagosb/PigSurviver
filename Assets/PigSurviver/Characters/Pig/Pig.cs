using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public class Pig : ALifeEntity, IDamagable
{
    
    [SerializeField]
    private float _speed;

    private const float SpeedUpTime = 1f;
    private const float SpeedUpIncrease = 2f;


    private Rigidbody2D _rigidBody2d;

    private Animator _anim;
    private static readonly int VelocityX = Animator.StringToHash("VelocityX");
    private static readonly int VelocityY = Animator.StringToHash("VelocityY");

    private SpriteRenderer _sprite;

    public Rigidbody2D RigidBody2D => _rigidBody2d;

    public float Speed => _speed;
    
    [SerializeField]
    private UnityEvent<Vector2> _moved;
    public UnityEvent<Vector2> Moved => _moved;

    [SerializeField] private UnityEvent _stopped;
    
    [SerializeField]
    private GameObject _bombPrefab;

    [SerializeField] 
    private GameObject _shitPrefab;

    [SerializeField]
    private GameObject _meat;

    public GameObject Meat => _meat;
    
    private bool _isDamaged;


    private event UnityAction<int> EventDamaged;
    public event UnityAction<int> Damaged
    {
        add => EventDamaged += value;
        remove => EventDamaged -= value;
    }


    public UnityEvent Stopped => _stopped;
    
    private void Start()
    {
        Life = 3;
        
        // Pig idle animation
        transform.DOLocalRotate(new Vector3(0, 0, -8), .8f)
            .SetEase(Ease.InBounce)
            .SetLoops(-1, LoopType.Yoyo);
        transform.DOScale(new Vector3(1.1f, 1.1f), 2f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
        
        EventDamaged += OnDamaged;
    }

    private void OnDamaged(int damage)
    {
        StartCoroutine(SpeedUpOnTime());
    }

    private IEnumerator SpeedUpOnTime()
    {
        _speed += SpeedUpIncrease;
        yield return new WaitForSecondsRealtime(SpeedUpTime);
        _speed -= SpeedUpIncrease;
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>(); 
        _rigidBody2d = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void Move(Vector2 direction){
        _anim.SetFloat(VelocityX, direction.x);
        _anim.SetFloat(VelocityY, direction.y);
        if(!IsStaying(direction))
        {
            var position = transform.position;
            _rigidBody2d.MovePosition(Vector2.Lerp(position, (Vector2)position + direction, Time.deltaTime * _speed));
            Moved.Invoke(direction);
            return;
        }
        Stopped.Invoke();
    }

    public bool IsStaying(Vector2 direction)
    {
        return direction.x == 0 && direction.y == 0;
    }

    public void ToDamage(int damage)
    {
        if (_isDamaged)
        {
            return;
        }
        Life -= damage;
        EventDamaged(damage);
        StartCoroutine(BlockDamage());
    }

    public IEnumerator BlockDamage()
    {
        _isDamaged = true;
        yield return new WaitForSecondsRealtime(2f);
        _isDamaged = false;
    }

    public void CreateBomb()
    {
        Instantiate(_bombPrefab, transform.position, Quaternion.identity);
    }

    public void CreateShit()
    {
        Instantiate(_shitPrefab, transform.position, Quaternion.identity);
    }
}
