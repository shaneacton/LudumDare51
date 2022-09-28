using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEditor;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private const string _LeaderboardName = "Score";

    public static LeaderboardManager instance;
    private string computerId;

    public int LeaderboardSize = 10;
    public List<PlayerLeaderboardEntry> leaderboard;


    private void Awake()
    {
        if (instance != null) { Destroy(this); }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        computerId = PlayerPrefs.GetString("ComputerID"); ;

        if (computerId == "")
        {
            computerId = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString("ComputerID", computerId);
        }

        Login();
    }

    void Login()
    {
        Debug.Log($"Logging in {computerId}");
        var request = new LoginWithCustomIDRequest
        {
            CustomId = computerId,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request,
            response => { Debug.Log("Successful login"); },
            response => { Debug.Log($"Leaderboard update failed {response.GenerateErrorReport()}"); }
        );
    }

    public void SendScore(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = _LeaderboardName,   // leaderboard name
                    Value = score                       // actual score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(
            request,
            response => { Debug.Log("Leaderboard update success"); },
            response => { Debug.Log($"Leaderboard update failed {response.GenerateErrorReport()}"); }
        );
    }

    public void GetLeaderboard(Action<GetLeaderboardResult> resultCallback)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = _LeaderboardName,
            MaxResultsCount = 10,
        };

        PlayFabClientAPI.GetLeaderboard(
            request,
            resultCallback,
            response => { Debug.Log($"Get leaderboard failed {response.GenerateErrorReport()}"); }
        );
    }

    public void UpdateName(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name,
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(
            request,
            response => { Debug.Log($"Successful name change: {name}"); },
            response => { Debug.Log($"Name update failed {response.GenerateErrorReport()}"); }
        );
    }
}