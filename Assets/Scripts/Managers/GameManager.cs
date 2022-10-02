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
    public Texture2D cursorTexture;
    public int score = 0;
    private List<List<MovementData>> _ghostMovements = new List<List<MovementData>>();
    private List<Ghost> _ghosts = new List<Ghost>();

    [System.NonSerialized]
    public List<Bullet> _bullets = new List<Bullet>();

    [System.NonSerialized]
    public bool canMove = true;
    private Tile nearestSpawnToPlayer;
    private bool movingPlayerToTarget = false;
    public float spawnMoveSpeed = 4f;

    private void Awake()
    {
        instance = this;
        movementRecorder = player.GetComponent<MovementRecorder>();
        // deadUI.SetActive(false);
        playerCollider = player.GetComponent<Collider2D>();
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
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
            Tile nearestSpawnToPlayer = SpawnManager.instance.getNearestSpawnPoint(player);
            foreach (Tile spawnPoint in SpawnManager.instance.SpawnPoints)
            {
                if (spawnPoint == nearestSpawnToPlayer)
                {
                    spawnPoint.setColour(Color.white);
                }
                else
                {
                    spawnPoint.setColour(Color.grey);
                }
            }

        }
    }

    public static void incrementScore(int num=1)
    {
        instance.score+=num;
        instance.scoreUI.SetText($"Score: {instance.score}");
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
        Destroy(enemy);
    }

    public void OnPlayerDead()
    {
        alive = false;
        LeaderboardManager.instance.SendScore(score);
        StartCoroutine(showDeadUI());
        HUD.SetActive(false);
        
        AudioManager.Play("PlayerDeath");
        AudioManager.Play("MenuSong");

        player.transform.GetChild(0).GetComponent<Animator>().SetBool("isDead", true);

        // StartCoroutine(SwitchScene());
    }

    IEnumerator showDeadUI(){
        yield return new WaitForSeconds(1f);
        Destroy(player);
        deadUI.SetActive(true);
    }

    public Tile OnStart()
    {
        movementRecorder.StopRecording();
        AudioManager.Stop("MenuSong");
        // AudioManager.Play("TenSecSong");

        Tile nearestSpawn = SpawnManager.instance.getNearestSpawnPoint(player);
        Vector3 pos = nearestSpawn.transform.position;
        pos.z = 0;
        player.transform.position = pos;

        return nearestSpawn;
    }


    public Tile OnReset()
    {
        // Start of break
        // movementRecorder.StopRecording();
        AudioManager.Play("Rewind");

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

        foreach (var g in _ghosts) { g.ReverseMovement(); }

        foreach (var b in _bullets)
        {
            try { Destroy(b.gameObject); } catch (Exception e) {Debug.LogError(e);}
        }

        nearestSpawnToPlayer = SpawnManager.instance.getNearestSpawnPoint(player);
        movingPlayerToTarget = true;
        // player.transform.position = nearestSpawn.position;

        return nearestSpawnToPlayer;
    }

    public void onBreakEnd()
    {
        // movementRecorder.StartRecording();
        AudioManager.Play("TenSecSong");
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

    public void EnablePlayerMovement()
    {
        canMove = true;
        movementRecorder.StartRecording();
    }

    public void DisablePlayerMovement()
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
