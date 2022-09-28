using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public TMP_InputField NameField;
    public Button playButton;

    public string playerName;

    public List<TextMeshProUGUI> _LeaderboardUI;


    void Awake()
    {
        // var compId = PlayerPrefs.GetString("ComputerID");
        playerName = PlayerPrefs.GetString("PlayerName");

    }

    private void Start()
    {
        instance = this;
    }

    private void OnEnable()
    {
        if (playerName == "")
            playButton.interactable = false;

        StartCoroutine(GetLeaderboard());
    }

    IEnumerator GetLeaderboard()
    {
        yield return new WaitForSeconds(2);
        LeaderboardManager.instance.GetLeaderboard(DisplayLeaderboard);
    }

    void DisplayLeaderboard(GetLeaderboardResult result)
    {
        Debug.Log("Got leaderboard succesfully");
        for (int i = 0; i < result.Leaderboard.Count; i++)
        {
            var item = result.Leaderboard[i];
            var displayStr = $"{item.Position}.  {item.DisplayName}: {item.StatValue}";
            _LeaderboardUI[i].SetText(displayStr);
            Debug.Log(displayStr);
        }
    }

    public void SetName()
    {
        if (NameField.text == "") { return; }

        playerName = NameField.text;
        PlayerPrefs.SetString("PlayerName", playerName);

        LeaderboardManager.instance.UpdateName(playerName);

        playButton.interactable = true;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
}
