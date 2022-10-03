using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    private Collider2D playerCollider;
    public GameObject ghostPrefab;
    public GameObject coinPrefab;
    public bool alive = true;
    private MovementRecorder movementRecorder;
    public GameObject deadUI;
    public GameObject HUD;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI waveUI;
    public Texture2D cursorTexture;
    public int score = 0;
    private int wave = 0;
    private List<List<MovementData>> _ghostMovements = new List<List<MovementData>>();
    private List<Ghost> _ghosts = new List<Ghost>();

    [System.NonSerialized]
    public List<Bullet> _bullets = new List<Bullet>();
    public bool canMove = true;
    private Tile nearestSpawnToPlayer;
    private bool movingPlayerToTarget = false;
    public float spawnMoveSpeed = 4f;

    public PostProcessing postProcessing;

    private void Awake()
    {
        instance = this;
        movementRecorder = player.GetComponent<MovementRecorder>();
        // deadUI.SetActive(false);
        playerCollider = player.GetComponent<Collider2D>();
        // Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        // ;Cursor.visible = false
    }

    private void Update()
    {
        if (!alive) return;
        if (movingPlayerToTarget)
        {
            playerCollider.enabled = false;
            movePlayerToTarget(nearestSpawnToPlayer.transform.position, spawnMoveSpeed);
        }
        else
        {
            playerCollider.enabled = true;

            // if (!alive) { return; }
            // Tile nearestSpawnToPlayer = SpawnManager.instance.getNearestSpawnPoint(player);
            // foreach (Tile spawnPoint in SpawnManager.instance.SpawnPoints)
            // {
            //     if (spawnPoint == nearestSpawnToPlayer)
            //         spawnPoint.setColour(new Color(0.65f, 0.65f, 0.65f, 1f));
            //     else
            //         spawnPoint.setColour(Color.grey);
            // }
        }
    }

    public static void incrementScore(int num = 1)
    {
        instance.score += num;
        instance.scoreUI.SetText($"Score: {instance.score}");
    }

    public void incrementWave(int num = 1)
    {
        wave += num;
        waveUI.SetText($"Wave: {instance.wave}");
    }
    public void OnKillEnemy(GameObject enemy)
    {

        float coinSpawnFac = Random.Range(0f, 1f);
        // Debug.Log("coin spawn fac: " + coinSpawnFac);
        if (coinSpawnFac < Enemy.spawnChance)
        {
            // Debug.Log("spawning coin at: " + enemy.transform.position);
            Instantiate(coinPrefab, enemy.transform.position, enemy.transform.rotation);
        }
        AudioManager.Play("EnemyDeath");
        incrementScore();
        enemy.GetComponent<Enemy>().isDead = true;
        enemy.GetComponent<Collider2D>().enabled = false;
        enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        enemy.GetComponent<Animator>().SetBool("isDead", true);
        StartCoroutine(DestroyEnemy(enemy));
    }

    IEnumerator DestroyEnemy(GameObject enemy)
    {
        yield return new WaitForSeconds(1);
        Destroy(enemy);
    }

    public void OnPlayerDead()
    {
        alive = false;
        canMove = false;

        LeaderboardManager.instance.SendScore(score);
        StartCoroutine(showDeadUI());
        HUD.SetActive(false);

        AudioManager.Play("PlayerDeath");
        AudioManager.Play("MenuSong");

        player.transform.GetChild(0).GetComponent<Animator>().SetBool("isDead", true);

        // StartCoroutine(SwitchScene());
        Cursor.visible = true;
    }

    IEnumerator showDeadUI()
    {
        yield return new WaitForSeconds(1f);
        Destroy(player);
        deadUI.SetActive(true);
    }

    public Tile OnStart()
    {
        // Cursor.visible = false;

        AudioManager.Stop("MenuSong");

        if (!alive) { return new Tile(); }

        DisablePlayerMovementAndStopRecording();

        Tile nearestSpawn = SpawnManager.instance.getNearestSpawnPoint(player);
        Vector3 pos = nearestSpawn.transform.position;
        Node tilePos = MapManager.getTileLocation(pos);
        pos.z = 1 + (tilePos.y / (float)MapManager.singleton.mapDef.numYTiles);
        player.transform.position = pos;

        return nearestSpawn;
    }


    public Tile OnReset()
    {
        // Start of break
        AudioManager.Play("Rewind");

        // Spawn new ghost
        var ghostMovement = new List<MovementData>(movementRecorder.movements);
        _ghostMovements.Add(ghostMovement);
        movementRecorder.movements.Clear();

        var ghostGO = Instantiate(
            ghostPrefab,
            ghostMovement[ghostMovement.Count - 1].position,
            ghostMovement[ghostMovement.Count - 1].rotation
        );
        var ghost = ghostGO.GetComponent<Ghost>();
        ghost.movements = ghostMovement;
        _ghosts.Add(ghost);

        // put ghosts in reverse mode
        foreach (var g in _ghosts) { g.ReverseMovement(); }

        // Destroy all bullets
        foreach (var b in _bullets)
        { try { Destroy(b.gameObject); } catch (Exception e) { Debug.LogError(e); } }

        // Move player to new spawn point
        if (alive)
        {
            DisablePlayerMovementAndStopRecording();
            nearestSpawnToPlayer = SpawnManager.instance.getNearestSpawnPoint(player);
            movingPlayerToTarget = true;
            player.GetComponent<Attack>().chargeUpBar.gameObject.SetActive(false);
        }

        return nearestSpawnToPlayer;
    }

    public void onBreakEnd()
    {
        postProcessing.lerpStart = Time.time;
        AudioManager.Play("TenSecSong");
        incrementWave();

        foreach (var g in _ghosts) { g.ResetPosition(); }
    }

    private void movePlayerToTarget(Vector3 target, float speed)
    {
        target.z = 0;
        if (player.transform.position != target)
        {
            var step = speed * Time.deltaTime;
            player.transform.position = Vector3.MoveTowards(player.transform.position, target, step);
        }
        else
        {
            movingPlayerToTarget = false;
        }
    }

    public void EnablePlayerMovementAndStartRecording()
    {
        canMove = true;
        movementRecorder.StartRecording();
    }

    public void DisablePlayerMovementAndStopRecording()
    {
        canMove = false;
        movementRecorder.StopRecording();
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
    }

    public static long getEpochTime()
    {
        System.DateTime epochStart = new System.DateTime(2020, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        return cur_time;
    }
}
