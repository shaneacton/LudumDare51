using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    List<MovementData> movements;
    private int _i;
    void Start()
    {
        transform.position = movements[0].position;
        transform.rotation = movements[0].rotation;
        _i = 1;
    }

    void FixedUpdate()
    {
        transform.position = movements[_i].position;
        transform.rotation = movements[_i].rotation;
        // TODO shooting

        _i++;
    }
}
