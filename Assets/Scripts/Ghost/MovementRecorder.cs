using System.Collections.Generic;
using UnityEngine;

class MovementRecorder : MonoBehaviour
{
    public List<MovementData> movements = new List<MovementData>();
    private bool _attacked = false;
    private void Awake()
    {
        movements = new List<MovementData>();
        movements.Add(new MovementData(transform.position, transform.rotation, false));
    }
    private void FixedUpdate()
    {
        movements.Add(new MovementData(transform.position, transform.rotation, _attacked));
        _attacked = false;
    }

    public void Attacked()
    {
        _attacked = true;
    }
}
