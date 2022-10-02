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

    void Awake()
    {
        instance = this;

        playerName = PlayerPrefs.GetString("PlayerName");
        if (playerName == "")
            playerName = SystemInfo.deviceName;
    }

    private void OnEnable()
    {
        LeaderboardManager.instance.GetLeaderboard(DisplayLeaderboard);
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

        leaderboard.Insert(0, "<b>Pos.\tName: Score</b>\n_________________");
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
