using System.Collections.Generic;
using UnityEngine;

class MovementRecorder : MonoBehaviour
{
    public List<MovementData> movements = new List<MovementData>();
    private bool _attacked = false;
    private bool _canRecord = true;
    private void Awake()
    {
        if (!_canRecord) { return; }

        movements = new List<MovementData>();
    }
    private void FixedUpdate()
    {
        if (!_canRecord) { return; }

        movements.Add(new MovementData(transform.position, transform.rotation, _attacked));
        _attacked = false;
    }

    public void Attacked() { _attacked = true; }

    public void StartRecording() { _canRecord = true; }

    public void StopRecording() { _canRecord = false; }
}
