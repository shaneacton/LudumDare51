using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public bool alive = true;

    public TextMeshProUGUI deadUI;
    public TextMeshProUGUI scoreUI;
    private int score = 0;

    void Start()
    {
        instance = this;
        deadUI.enabled = false;

        player = GameObject.Find("Player");
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

    public void OnReset(){

        Transform nearestSpawn = SpawnManager.instance.getNearestSpawnPoint(player);
        player.transform.position = nearestSpawn.position;

    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
    }
}
