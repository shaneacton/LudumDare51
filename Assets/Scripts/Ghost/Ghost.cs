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
        if (movements.Count != 0 && _i < movements.Count - 1)
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

            _i = Mathf.Max(_i - rewindSpeed, 0);
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
            if (nextAttack < 2 * 50 * indicatorSeconds && nextAttack != -1)
            {
                renderer.material.SetColor("_Color", new Color(1f, 0.25f, 0.25f, 0.65f));
            }
            else
            {
                renderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 0.5f));
            }

            if (nextAttack < 50 * indicatorSeconds && nextAttack != -1)
            { // less than 50 frames from now
                warningIndicator.SetActive(true);
                float scale = warningIndicatorGrowSpeed * nextAttack * Time.deltaTime;
                warningIndicator.transform.localScale += new Vector3(scale, scale, scale);
            }
            else
            { // not attacking soon
                warningIndicator.SetActive(false);
                warningIndicator.transform.localScale = originalWarningIndicatorScale;
            }

            _i = Mathf.Min(_i + 1, movements.Count - 1);
        }
        Vector3 pos = transform.position;
        Node tilePos = MapManager.getTileLocation(pos - new Vector3(0, 0.1f, 0));
        pos.z = 1 + (tilePos.y / (float)MapManager.singleton.mapDef.numYTiles);
        transform.position = pos;
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
