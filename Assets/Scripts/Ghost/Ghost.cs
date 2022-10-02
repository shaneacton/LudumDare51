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

    public int indicatorSeconds;
    private int nextAttack;
    public GameObject warningIndicator;
    private Vector3 originalWarningIndicatorScale;
    public float warningIndicatorGrowSpeed = 0.001f;

    public Renderer renderer;

    public Animator animator;

    private State state;

    enum State
    {
        Normal, Reverse
    }

    void Start()
    {
        _attack = GetComponent<GhostAttack>();
        originalWarningIndicatorScale = warningIndicator.transform.localScale;
    }

    void Update()
    {
        transform.GetChild(0).transform.rotation = Quaternion.identity;
    }


    void FixedUpdate()
    {
        if (movements.Count != 0 && _i < movements.Count)
        {
            animator.SetBool("isMoving", Vector3.Distance(transform.position, movements[_i].position) >= 0.01);
            animator.SetFloat("direction", transform.rotation.z * 180);
        }

        if (state == State.Normal) { FollowMovements(); }
        else if (state == State.Reverse) { ReverseMovements(); }
    }

    private void ReverseMovements()
    {
        renderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 0.4f));
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

            if (step.attackType == AttackType.Shoot) { _attack.fire(); }
            else if (step.attackType == AttackType.Lazer) { _attack.lazer(); }

            nextAttack = movements.GetRange(_i, movements.Count - _i - 1).FindIndex((m) => m.attackType != AttackType.Nothing);
            if (nextAttack < 50 * this.indicatorSeconds && nextAttack != -1)
            { // less than 50 frames from now
                warningIndicator.SetActive(true);
                float scale = warningIndicatorGrowSpeed * nextAttack * Time.deltaTime;
                warningIndicator.transform.localScale += new Vector3(scale, scale, scale);
                renderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 0.7f));
            }
            else
            { // not attacking soon
                warningIndicator.SetActive(false);
                warningIndicator.transform.localScale = originalWarningIndicatorScale;
                renderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 0.4f));
            }

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
