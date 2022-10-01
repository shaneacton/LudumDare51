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

    void Start()
    {
        _attack = GetComponent<GhostAttack>();

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
            var step = movements[_i];
            transform.position = step.position;
            transform.rotation = step.rotation;
            if (step.attacked)
                _attack.fire();

            _i++;
        }
        
        // TODO shooting
    }

    public void SetMovements(List<MovementData> mvment)
    {
        movements = mvment;
        Debug.Log(movements.Count);
    }

    public void ResetMovement()
    {
        _i = 0;
    }
}
