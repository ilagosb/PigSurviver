using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private ALifeEntity _lifeEntity;

    private List<GameObject> _health = new List<GameObject>();

    private int _currentLife;

    [SerializeField]
    private float _offsetBetweenHealthView;

    [SerializeField]
    private GameObject _healthView;
    
    private void Awake()
    {
        _lifeEntity.LifeCountChanged += UpdateView;
    }

    private void Init()
    {
        _currentLife = _lifeEntity.GetLife();
        var sizeView = _healthView.GetComponent<SpriteRenderer>().bounds.size;
        var startX = transform.position.x - (sizeView.x * (_currentLife / 2) - (_offsetBetweenHealthView * (_currentLife / 2)));
        Vector2 currentPosition = new Vector2(startX, transform.position.y);
        for(int i = 0; i < _currentLife; i++)
        {
            var health = Instantiate(_healthView, currentPosition, Quaternion.identity);
            health.transform.parent = transform;
            currentPosition.x += sizeView.x + _offsetBetweenHealthView;
            _health.Add(health);
        }
    }

    private void UpdateView(int oldValue, int newValue)
    {
        if (_health.Count == 0)
        {
            Init();
        }
        else
        {
            _currentLife = newValue;
            var healthPoint = _health[oldValue - 1];
            var spriteRender = healthPoint.GetComponent<SpriteRenderer>();
            spriteRender.DOFade(0, .7f).SetEase(Ease.InQuad);
            healthPoint.transform.DOMoveY(healthPoint.transform.position.y - .19f, .6f).SetEase(Ease.OutCubic);
        }
    }
}