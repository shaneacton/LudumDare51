using System.Collections.Generic;
using UnityEngine;

class MovementRecorder : MonoBehaviour
{
    public List<MovementData> movements;
    private float _worldTime = 0;
    private bool _attacked = false;
    private bool _once = true;
    public Ghost ghost;
    private void Start()
    {
        movements = new List<MovementData>();
        movements.Add(new MovementData(transform.position, transform.rotation, false));
    }
    private void FixedUpdate()
    {
        movements.Add(new MovementData(transform.position, transform.rotation, _attacked));
        _attacked = false;

        _worldTime += Time.fixedDeltaTime;

        if (_worldTime > 2 && _once)
        {
            ghost.movements = movements;
            _once = false;
        }
    }

    public void Attacked()
    {
        _attacked = true;
    }
}
