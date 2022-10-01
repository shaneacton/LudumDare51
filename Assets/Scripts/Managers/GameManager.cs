using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    private Collider2D playerCollider;
    public GameObject ghostPrefab;
    public bool alive = true;
    private MovementRecorder movementRecorder;
    public TextMeshProUGUI deadUI;
    public TextMeshProUGUI scoreUI;
    private int score = 0;
    private List<List<MovementData>> _ghostMovements = new List<List<MovementData>>();
    private List<Ghost> _ghosts = new List<Ghost>();

    [System.NonSerialized]
    public List<Bullet> _bullets = new List<Bullet>();

    [System.NonSerialized]
    public bool canMove = true;
    private Transform nearestSpawnToPlayer;
    private bool movingPlayerToTarget = false;
    public float spawnMoveSpeed = 4f;

    private void Awake()
    {
        instance = this;
        movementRecorder = player.GetComponent<MovementRecorder>();
        deadUI.enabled = false;
        playerCollider = player.GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (movingPlayerToTarget)
        {
            playerCollider.enabled = false;
            movePlayerToTarget(nearestSpawnToPlayer.position, spawnMoveSpeed);
        }
        else
        {
            playerCollider.enabled = true;
        }
    }

    public void OnKillEnemy()
    {
        score++;
        scoreUI.SetText($"Score: {score}");
    }

    public void OnPlayerDead()
    {
        alive = false;
        deadUI.enabled = true;

        // LeaderboardManager.instance.SendScore(score);
        // StartCoroutine(SwitchScene());
    }

    public Transform OnStart()
    {
        movementRecorder.StopRecording();

        Transform nearestSpawn = SpawnManager.instance.getNearestSpawnPoint(player);
        Vector3 pos = nearestSpawn.position;
        pos.z = 0;
        player.transform.position = pos;

        return nearestSpawn;
    }


    public Transform OnReset()
    {
        // Start of break
        // movementRecorder.StopRecording();

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
            try { Destroy(b.gameObject); }catch(Exception e){}
        }

        nearestSpawnToPlayer = SpawnManager.instance.getNearestSpawnPoint(player);
        movingPlayerToTarget = true;
        // player.transform.position = nearestSpawn.position;

        return nearestSpawnToPlayer;
    }

    public void onBreakEnd()
    {
        // movementRecorder.StartRecording();
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
