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

    void Awake()
    {
        instance = this;

        playerName = PlayerPrefs.GetString("PlayerName");
        if (playerName == "")
            playerName = SystemInfo.deviceName;
    }

    private void OnEnable()
    {
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
            if (result.Leaderboard[i].Profile.PlayerId == LeaderboardManager.instance.playFabId)
            {
                leaderboard[i] = $"<b><color=white>{leaderboard[i]}</color></b>";
                break;
            }
        }

        leaderboard.Insert(0, "<b><color=white>Pos.\tName: Score</color></b>\n_________________");
        Debug.Log(_LeaderboardUI);
        _LeaderboardUI.SetText(string.Join('\n', leaderboard));
    }

    public void SetName()
    {
        Debug.Log($"Setting name {NameField.text}");
        if (NameField.text == "") { return; }

        playerName = NameField.text;
        PlayerPrefs.SetString("PlayerName", playerName);

        LeaderboardManager.instance.UpdateName(playerName);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
}
