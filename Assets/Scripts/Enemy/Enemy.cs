using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _player_rb;

    private Rigidbody2D _rb;
    public float speed = 10;

    private Renderer _renderer;

    public static float spawnChance = 0.3333f;

    public float lastSeenPlayerTime = -1;
    
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

    private void approachPlayer(float infrontMag = 2f)
    {
        if (!GameManager.instance.alive) return;
        var player_vel = _player_rb.velocity.normalized * infrontMag;
        var target = _player.transform.position + new Vector3(player_vel.x, player_vel.y, 0f);
        if (canEnemySeePlayer())
        {            
            hordeSafeMoveTowards(target);
        }
        else
        {
            aStarMoveTowards(target);
        }
    }

    void aStarMoveTowards(Vector3 target)
    {
        Vector3 nextTileTarget = MapManager.getNextTargetTilePos(this, target);
        // Debug.Log("moving to " + nextTile + " from pos: " + transform.position + " and tile: " + MapManager.getTileLocation(transform.position));
        hordeSafeMoveTowards(nextTileTarget, minimumDistance:0.15f);
    }

    private void hordeSafeMoveTowards(Vector3 target, float minimumDistance = 3f)
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
            hordeSafeMoveTowards(transform.position, minimumDistance: 0f);
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
        if(!GameManager.instance.alive) return false;
        Vector2 dir2Player = (GameManager.instance.player.transform.position - transform.position).normalized;
        Vector2 dirPerp = new Vector2(dir2Player.y, -dir2Player.x) * 0.5f;

        Vector2[] offsets = {-dirPerp, Vector2.zero, dirPerp};
        int layerMask = ~(LayerMask.GetMask("Enemy", "Ghost", "EnemyBullet", "PlayerBullet"));
        foreach (Vector2 offset in offsets)
        {
            dir2Player = ((Vector2) GameManager.instance.player.transform.position -
                          ((Vector2) transform.position + offset)).normalized;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + offset, dir2Player, Mathf.Infinity, layerMask);
            Debug.DrawRay((Vector2)transform.position + offset, dir2Player);
            bool hitPlayer = false;
            if (hit.collider != null)
            {
                GameObject hitObj = hit.collider.gameObject;
                // Debug.Log("can see " + hitObj);
                if (hitObj.CompareTag("Player"))
                {
                    hitPlayer = true;
                }
            }

            if (!hitPlayer)
            {
                return false;
            }
        }

        lastSeenPlayerTime = Time.time;
        return true;
    }

    private void moveTowards(Vector3 target)
    {
        Vector3 enemyToTarget = target - transform.position;
        if (enemyToTarget.magnitude >= speed * Time.fixedDeltaTime)
        { // can move full amount
            float speedMod = 1f;
            if (canEnemySeePlayer())
            {
                speedMod = 0.25f;
            }
            Vector3 newPos = transform.position + enemyToTarget.normalized * speed * speedMod * Time.fixedDeltaTime;
            _rb.MovePosition(newPos);
        }
        else
        { // will overshoot. Just move to target
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
