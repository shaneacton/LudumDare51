using System.Collections.Generic;
using UnityEngine;

public class MovementRecorder : MonoBehaviour
{
    List<MovementData> movements;

    private void Start()
    {
        movements = new List<MovementData>();
        movements.Add(new  MovementData(transform.position, transform.rotation, false));
    }
    private void FixedUpdate()
    {
        movements.Add(new MovementData(transform.position, transform.rotation, false));
    }
}