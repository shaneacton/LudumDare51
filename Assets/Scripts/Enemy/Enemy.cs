using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _player_rb;

    private Rigidbody2D _rb;
    public float speed = 10;

    private int _strategy;

    private Renderer _renderer;

    private Vector3 lastMovementDirection;

    void Start()
    {
        _player = GameManager.instance.player;
        _player_rb = _player.GetComponent<Rigidbody2D>();

        _rb = GetComponent<Rigidbody2D>();
        _strategy = Random.Range(0, 2);

        _renderer = GetComponent<Renderer>();
        
        EnemyManager.registerEnemy(this);
    }

    void FixedUpdate()
    {
        DestroyOutOfBounds();
        approachPlayer();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameManager.instance.OnPlayerDead();
            Destroy(other.gameObject);
        }
    }

    private void approachPlayer(float infrontMag=2f)
    {
        var player_vel = _player_rb.velocity.normalized * infrontMag;
        var target = _player.transform.position + new Vector3(player_vel.x, player_vel.y, 0f);
        // float distToTarget = Vector3.Distance(transform.position, _player.transform.position);
        safeMoveTowards(target);
    }

    private void safeMoveTowards(Vector3 target, float minimumDistance = 3f)
    {
        Vector3 enemyToTarget = target - transform.position;
        Vector3 newPos = transform.position + enemyToTarget.normalized * speed * Time.fixedDeltaTime;
        if (Vector3.Distance(newPos, target) < minimumDistance)
        { // too close, don't approach
            safeMoveTowards(transform.position, minimumDistance:0f);
            return;
        }
        
        List<Enemy> nearbyEnemies = EnemyManager.getNearbyEnemies(this);
        // nearbyEnemies.Add(this);
        if (Vector3.Distance(target, transform.position) > 0.001f)
        { // is moving
            foreach (Enemy nearby in nearbyEnemies)
            { // move around other enemies
                Vector3 nearToPos = newPos - nearby.transform.position;
                float dist = nearToPos.magnitude;
                // Debug.Log("dist: " + dist + " offsetting newPos by: " + nearToPos.normalized / Mathf.Pow(dist, 2f));
                newPos += nearToPos.normalized / Mathf.Pow(dist, 3f);
            }
        }
        
        moveTowards(newPos);
    }

    private void moveTowards(Vector3 target)
    {
        Vector3 enemyToTarget = target - transform.position;
        if (Vector3.Dot(enemyToTarget, lastMovementDirection) < 0)
        { // has flipped direction since last frame
            lastMovementDirection = lastMovementDirection + Time.fixedDeltaTime * (enemyToTarget.normalized - lastMovementDirection);
            return;
        }
        if (enemyToTarget.magnitude >= speed * Time.fixedDeltaTime)
        {
            Vector3 newPos = transform.position + enemyToTarget.normalized * speed * Time.fixedDeltaTime;
            _rb.MovePosition(newPos);  
        }
        else
        {
            _rb.MovePosition(target);
        }

        lastMovementDirection = enemyToTarget.normalized;
    }

    private void DestroyOutOfBounds()
    {
        if (!GameManager.instance.alive)
            Destroy(gameObject);

        var pos = transform.position;
        if (Mathf.Abs(pos.x) > 12 || Mathf.Abs(pos.y) > 12)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EnemyManager.removeEnemy(this);
    }
}
