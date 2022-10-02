using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab.ClientModels;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EndUIManager : MonoBehaviour
{
    public static EndUIManager instance;
    public TMP_InputField NameField;

    public string playerName;

    public TextMeshProUGUI _LeaderboardUI;

    void Awake()
    {
        playerName = System.Environment.MachineName;
        instance = this;
    }

    private void OnEnable()
    {
        LeaderboardManager.instance.GetLeaderboard(DisplayLeaderboard);
    }

    IEnumerator GetLeaderboard()
    {
        yield return new WaitForSeconds(2);
        LeaderboardManager.instance.GetLeaderboard(DisplayLeaderboard);
    }

    public void DisplayLeaderboard(GetLeaderboardResult result)
    {
        Debug.Log("Got leaderboard succesfully" + result.Leaderboard.Count);

        result.Leaderboard.Sort((x, y) => x.Position.CompareTo(y.Position));
        var leaderboard = result.Leaderboard.Select(x => $"{x.Position}.\t{x.DisplayName}: {x.StatValue}").ToArray();

        Debug.Log(leaderboard.Length);

        for (int i = 0; i < leaderboard.Length - 1; i++)
        {
            if (result.Leaderboard[i].PlayFabId == LeaderboardManager.instance.computerId)
            {
                leaderboard[i] = $"<b>{leaderboard[i]}</b>";
                break;
            }
        }

        Debug.Log(leaderboard);
        Debug.Log(_LeaderboardUI);

        _LeaderboardUI.SetText(string.Join('\n', leaderboard));
    }

    public void SetName()
    {
        if (NameField.text == "") { return; }

        playerName = NameField.text;

        LeaderboardManager.instance.UpdateName(playerName);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
