using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioOnDamaged : MonoBehaviour
{
    [SerializeField]
    private AudioClip _source;
    
    private AudioSource _audio;
    
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        GetComponent<IDamagable>().Damaged += Play;
    }

    private void Play(int _)
    {
        _audio.clip = _source;
        _audio.loop = false;
        _audio.volume = 1f;
        _audio.Play();
    }
}
