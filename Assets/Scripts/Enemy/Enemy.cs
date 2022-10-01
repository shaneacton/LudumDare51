using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _player_rb;

    private Rigidbody2D _rb;
    public float speed = 10;
    
    private Renderer _renderer;
    
    void Start()
    {
        _player = GameManager.instance.player;
        _player_rb = _player.GetComponent<Rigidbody2D>();

        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
        
        EnemyManager.registerEnemy(this);
    }

    void FixedUpdate()
    {
        DestroyOutOfBounds();
        approachPlayer();
        if (canEnemySeePlayer())
        {
            _renderer.material.SetColor("_Color", Color.white);
            // Debug.Log("can see player");
        }
        else
        {
            _renderer.material.SetColor("_Color", Color.black);
            // Debug.Log("cannot see player");
        }
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
        aStarMoveTowards(target);
    }

    void aStarMoveTowards(Vector3 target)
    {
        Vector3 nextTileTarget = MapManager.getNextTarget(this, target);
        // Debug.Log("moving to " + nextTile + " from pos: " + transform.position + " and tile: " + MapManager.getTileLocation(transform.position));
        safeMoveTowards(nextTileTarget);
    }

    private void safeMoveTowards(Vector3 target, float minimumDistance = 0.15f)
    {
        /*
         * First makes sure enemy is not already too close
         * Then accounts for nearby enemies too
         */
        Vector3 enemyToTarget = target - transform.position;
        Vector3 newPos = transform.position + enemyToTarget.normalized * speed * Time.fixedDeltaTime;
        if (Vector3.Distance(newPos, target) < minimumDistance)
        { // too close, don't approach
            // Debug.Log("too close. Dist: " + Vector3.Distance(newPos, target));
            safeMoveTowards(transform.position, minimumDistance:0f);
            return;
        }
        
        if (Vector3.Distance(target, transform.position) > 0.001f)
        { // is moving
            List<Enemy> nearbyEnemies = EnemyManager.getNearbyEnemies(this);
            foreach (Enemy nearby in nearbyEnemies)
            { // move around other enemies
                Vector3 nearToPos = newPos - nearby.transform.position;
                float dist = nearToPos.magnitude;
                // Debug.Log("dist: " + dist + " offsetting newPos by: " + nearToPos.normalized / Mathf.Pow(dist, 2f));
                newPos += 0.002f * nearToPos.normalized / Mathf.Pow(dist, 2f);
            }
        }
        
        moveTowards(newPos);
    }

    public bool canEnemySeePlayer()
    {
        Vector3 dir2Player = GameManager.instance.player.transform.position -transform.position;
        int layerMask = ~(LayerMask.GetMask("Enemy", "Ghost", "EnemyBullet", "PlayerBullet"));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir2Player, Mathf.Infinity, layerMask);
        
        if (hit.collider != null)
        {
            GameObject hitObj = hit.collider.gameObject;
            // Debug.Log("can see " + hitObj);
            if (hitObj.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    private void moveTowards(Vector3 target)
    {
        Vector3 enemyToTarget = target - transform.position;
        if (enemyToTarget.magnitude >= speed * Time.fixedDeltaTime)
        {
            Vector3 newPos = transform.position + enemyToTarget.normalized * speed * Time.fixedDeltaTime;
            _rb.MovePosition(newPos);  
        }
        else
        {
            _rb.MovePosition(target);
        }
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
