using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Ghost : MonoBehaviour
{
    public List<MovementData> movements = new List<MovementData>();
    private int _i;

    private float _worldTime;
    private bool _once;
    void Start()
    {
        if (movements.Count != 0)
        {
            transform.position = movements[0].position;
            transform.rotation = movements[0].rotation;
            _i = 1;
        }
    }

    void FixedUpdate()
    {
        if (movements.Count != 0)
        {
            transform.position = movements[_i].position;
            transform.rotation = movements[_i].rotation;
            _i++;
        }
        
        // TODO shooting
    }
}
