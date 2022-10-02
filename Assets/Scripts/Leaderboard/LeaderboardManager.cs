using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEditor;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private const string _LeaderboardName = "Score";

    public static LeaderboardManager instance;
    public string computerId;
    public string playFabId;

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
        computerId = SystemInfo.deviceUniqueIdentifier;
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
            response =>
            {
                Debug.Log("Successful login");
                playFabId = response.PlayFabId;
                GetLeaderboard(EndUIManager.instance.DisplayLeaderboard);
            },
            response => { Debug.Log($"Leaderboard update failed {response.GenerateErrorReport()}"); }
        );
    }

    public void SendScore(int score)
    {
        Debug.Log("Sending score");
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
            response =>
            {
                Debug.Log("Leaderboard update success");
                GetLeaderboard(EndUIManager.instance.DisplayLeaderboard);
            },
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