using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shit : MonoBehaviour
{
    [SerializeField]
    private AudioClip _stepSound;

    private AudioSource _audio;

    private bool _isBreak;
    
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_isBreak) return;
        Debug.Log(other + " in shit");
        if(other.TryGetComponent(out Enemy enemy))
        {
            _isBreak = true;
            enemy.IsDirty = true;
            _audio.clip = _stepSound;
            _audio.Play();
            Destroy(gameObject, .2f);
        }
    }
}
