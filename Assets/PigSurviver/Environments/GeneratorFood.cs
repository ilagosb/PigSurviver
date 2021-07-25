using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorFood : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _foodPlaces;

    [SerializeField]
    private Food _food;
    
    public Food Generate()
    {
        var place = _foodPlaces[Random.Range(0, _foodPlaces.Count)];
        return Instantiate(_food, place.position, Quaternion.identity);
    }
}
