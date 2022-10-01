using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    private Ghost currGhost;
    public GameObject ghostPrefab;
    public bool alive = true;
    private MovementRecorder movementRecorder;

    public TextMeshProUGUI deadUI;
    public TextMeshProUGUI scoreUI;
    private int score = 0;

    private List<List<MovementData>> _ghostMovements = new List<List<MovementData>>();
    private List<Ghost> _ghosts = new List<Ghost>();

    public bool canMove = true;

    private void Awake(){
        instance = this;
        movementRecorder = player.GetComponent<MovementRecorder>();
        deadUI.enabled = false;
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

        LeaderboardManager.instance.SendScore(score);
        StartCoroutine(SwitchScene());
    }

    public Transform OnStart()
    {
        Transform nearestSpawn = SpawnManager.instance.getNearestSpawnPoint(player);
        player.transform.position = nearestSpawn.position;

        return nearestSpawn;
    }


    public Transform OnReset()
    {
        var ghostMovement = new List<MovementData>(movementRecorder.movements);
        _ghostMovements.Add(ghostMovement);
        movementRecorder.movements.Clear();

        var ghostGO = Instantiate(ghostPrefab, ghostMovement[0].position, ghostMovement[0].rotation);
        var ghost = ghostGO.GetComponent<Ghost>();
        ghost.movements = ghostMovement;
        _ghosts.Add(ghost);

        foreach (var g in _ghosts) { g.ResetMovement(); }
        Transform nearestSpawn = SpawnManager.instance.getNearestSpawnPoint(player);
        player.transform.position = nearestSpawn.position;

        return nearestSpawn;
    }

    public void onBreakEnd(){

    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
    }

    public static long getEpochTime(){
        System.DateTime epochStart = new System.DateTime(2020, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        return cur_time;
    }
}
