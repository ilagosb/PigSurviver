using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorEnemy : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _enemyPlaces;

    [SerializeField]
    private List<Enemy> _enemies;
    
    public Enemy Generate()
    {
        var place = _enemyPlaces[Random.Range(0, _enemyPlaces.Count)];
        var enemy = _enemies[Random.Range(0, _enemies.Count)];
        return Instantiate(enemy, place.position, Quaternion.identity);
    }
}
