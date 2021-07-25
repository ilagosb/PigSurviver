using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _dirt;

    private Vector2 _cacheDirection = Vector2.right;
    
    public void OnMove(Vector2 direction)
    {
        if(!_cacheDirection.Equals(direction))
        {
            var shapeModule = _dirt.shape;
            var mainModule = _dirt.main;
            var rotationX = Vector3.Angle(_cacheDirection, direction);
            rotationX = rotationX / Math.Abs(rotationX / 90);
            mainModule.gravityModifier = direction.y != 0 ? direction.y : 1;
            shapeModule.rotation = new Vector3(rotationX * direction.y , rotationX * direction.x * -1);
            _dirt.Play();
            _cacheDirection = direction;
        }
    }

    public void OnStopped()
    {
        if (!_dirt.isPlaying) return;
        _dirt.Clear();
        _dirt.Stop();
    }
}
