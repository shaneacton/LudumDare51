using System.Collections.Generic;
using UnityEngine;

class MovementRecorder : MonoBehaviour
{
    public List<MovementData> movements = new List<MovementData>();
    private AttackType _attackType = AttackType.Nothing;
    private bool _canRecord = true;
    private void Awake()
    {
        if (!_canRecord) { return; }

        movements = new List<MovementData>();
    }
    private void FixedUpdate()
    {
        if (!_canRecord) { return; }

        movements.Add(new MovementData(transform.position, transform.rotation, _attackType));
        _attackType = AttackType.Nothing;
    }

    public void Attacked(AttackType attackType) { _attackType = attackType; }

    public void StartRecording() { _canRecord = true; }

    public void StopRecording() { _canRecord = false; }
}
