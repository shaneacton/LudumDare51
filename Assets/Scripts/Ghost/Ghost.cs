using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GhostAttack))]
class Ghost : MonoBehaviour
{
    public List<MovementData> movements = new List<MovementData>();
    private int _i;

    private float _worldTime;
    private bool _once;

    private GhostAttack _attack;

    private int rewindSpeed = 4;

    private State state;

    enum State
    {
        Normal, Reverse
    }

    void Start() { _attack = GetComponent<GhostAttack>(); }

    void FixedUpdate()
    {
        if (state == State.Normal) { FollowMovements(); }
        else if (state == State.Reverse) { ReverseMovements(); }
    }

    private void ReverseMovements()
    {
        if (movements.Count != 0 && _i < movements.Count && _i >= 0)
        {
            var step = movements[_i];
            transform.position = step.position;
            transform.rotation = step.rotation;
            // if (step.attacked)
            // _attack.fire();

            _i -= rewindSpeed;
        }
    }

    void FollowMovements()
    {
        if (movements.Count != 0 && _i < movements.Count)
        {
            var step = movements[_i];
            transform.position = step.position;
            transform.rotation = step.rotation;
            if (step.attacked)
                _attack.fire();

            _i++;
        }
    }


    public void SetMovements(List<MovementData> mvment)
    {
        movements = mvment;
        Debug.Log(movements.Count);
    }

    public void ReverseMovement()
    {
        state = State.Reverse;
        _i = movements.Count - 1;
    }

    public void ResetPosition()
    {
        _i = 1;
        state = State.Normal;
        transform.position = movements[0].position;
        transform.rotation = movements[0].rotation;
    }
}
