using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _player_rb;

    private Rigidbody2D _rb;
    public float speed = 10;

    private int _strategy;
    // For straight line strategy
    private bool _targetCalculated = false;
    private Vector3 _staticTarget;

    void Start()
    {
        _player = GameManager.instance.player;
        _player_rb = _player.GetComponent<Rigidbody2D>();

        _rb = GetComponent<Rigidbody2D>();
        _strategy = Random.Range(0, 3);
    }

    void FixedUpdate()
    {
        DestroyOutOfBounds();

        if (_strategy == 0)
            GotoPlayer();
        else if (_strategy == 1)
            InfrontOfPlayer();
        else if (_strategy == 2)
            StraightLine();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameManager.instance.OnPlayerDead();
            Destroy(other.gameObject);
        }
    }

    private void GotoPlayer()
    {
        var dir = _player.transform.position - transform.position;
        var newPos = transform.position + dir.normalized * speed * Time.fixedDeltaTime;

        _rb.MovePosition(newPos);
    }

    private void InfrontOfPlayer()
    {
        var player_vel = _player_rb.velocity;
        var target = _player.transform.position + new Vector3(player_vel.x, player_vel.y, 0f);

        var dir = target - transform.position;
        var newPos = transform.position + dir.normalized * speed * Time.fixedDeltaTime;

        _rb.MovePosition(newPos);
    }

    private void StraightLine()
    {
        if (!_targetCalculated)
        {
            speed *= 5; // straight line strategy requires faster movement

            var pos = transform.position;

            var x_noise = Random.Range(-1f, 1f);
            var y_noise = Random.Range(-1f, 1f);

            _staticTarget = new Vector3(-pos.x + x_noise, -pos.y + y_noise, 0);

            // Only do this once
            _targetCalculated = true;
        }

        var dir = _staticTarget - transform.position;
        var newPos = transform.position + dir.normalized * speed * Time.fixedDeltaTime;

        _rb.MovePosition(newPos);
    }

    private void DestroyOutOfBounds()
    {
        if (!GameManager.instance.alive)
            Destroy(gameObject);

        var pos = transform.position;
        if (Mathf.Abs(pos.x) > 10 || Mathf.Abs(pos.y) > 7)
            Destroy(this);
    }
}
