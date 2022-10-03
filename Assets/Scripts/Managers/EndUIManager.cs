using System.Collections;
using System.Linq;
using PlayFab.ClientModels;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndUIManager : MonoBehaviour
{
    public static EndUIManager instance;
    public TMP_InputField NameField;

    public string playerName;

    public TextMeshProUGUI _LeaderboardUI;
    public TextMeshProUGUI ScoreUI;

    public GameObject MainPanel;
    public GameObject ExplainPanel;

    void Awake()
    {
        instance = this;

        playerName = PlayerPrefs.GetString("PlayerName");
        if (playerName == "")
            playerName = SystemInfo.deviceName;

        if (playerName == "n/a")
            playerName = $"Player-{Random.Range(0, 1000)}";
    }

    private void OnEnable()
    {
        StartCoroutine(SetDefaultName());

        NameField.text = playerName;

        if (GameManager.instance == null) return;  // in main menu

        LeaderboardManager.instance.GetLeaderboard(DisplayLeaderboard);
        if (!GameManager.instance.alive)
        {
            ScoreUI.text = $"You scored: {GameManager.instance.score}";
            ScoreUI.enabled = true;
        }

    }

    public void DisplayLeaderboard(GetLeaderboardResult result)
    {
        Debug.Log("Got leaderboard succesfully" + result.Leaderboard.Count);

        result.Leaderboard.Sort((x, y) => x.Position.CompareTo(y.Position));
        var leaderboard = result.Leaderboard.Select(x => $"{x.Position}.\t{x.DisplayName}: {x.StatValue}").ToList();

        for (int i = 0; i < leaderboard.Count - 1; i++)
        {
            if (result.Leaderboard[i].PlayFabId == LeaderboardManager.instance.playFabId)
            {
                leaderboard[i] = $"<b><color=white>{leaderboard[i]}</color></b>";
                break;
            }
        }

        leaderboard.Insert(0, "<b><color=white>Pos.\tName: Score</color></b>\n_________________");
        _LeaderboardUI.SetText(string.Join('\n', leaderboard));
    }

    public IEnumerator SetDefaultName()
    {
        yield return new WaitForSeconds(1);
        LeaderboardManager.instance.UpdateName(playerName);
    }

    public void SetName()
    {
        Debug.Log($"Setting name {NameField.text}");
        if (NameField.text == "") { return; }

        playerName = NameField.text;
        PlayerPrefs.SetString("PlayerName", playerName);

        LeaderboardManager.instance.UpdateName(playerName);
    }

    public void PlayGame() { SceneManager.LoadScene("Game"); }

    public void QuitGame() { Application.Quit(); }

    public void DisplayExplaination()
    {
        ExplainPanel.SetActive(true);
        MainPanel.SetActive(false);
    }

    public void DisplayMainPanel()
    {
        ExplainPanel.SetActive(false);
        MainPanel.SetActive(true);
    }
}
