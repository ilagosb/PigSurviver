using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Food : MonoBehaviour
{

    private bool _isEaten = false;

    private AudioSource _audio;

    private event UnityAction EventEaten;
    
    public event UnityAction Eaten
    {
        add => EventEaten += value;
        remove => EventEaten += value;
    }

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        DOTween.Sequence()
            .Append(transform.DOMoveY(.15f, 1f).SetRelative())
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEaten) return;
        if(!other.TryGetComponent(out Pig _)) return;
        _isEaten = true;
        _audio.Play();
        EventEaten();
        Destroy(gameObject, .2f);
    }
}
