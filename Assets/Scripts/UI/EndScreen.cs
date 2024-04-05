using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EndScreen : MonoBehaviour
{
    public Button restartButton;
    public TextMeshProUGUI leaderboardText;
    
    private string leaderBoardURL = "https://advanced3dgd.000webhostapp.com/ca2-all-users.php";
    private List<(string, int)> leaderboardData = new List<(string, int)>();
    
    public void RestartGame()
    {
        string currentUsername = PlayerPrefs.GetString("name"), currentPassword = PlayerPrefs.GetString("password");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("name", currentUsername);
        PlayerPrefs.SetString("password", currentPassword);
        SceneManager.LoadScene(0);
        
    }

    private void Start()
    {
        GameManager.Instance.SavePlayerData();
        restartButton.onClick.AddListener(RestartGame);
        FetchLeaderboardData();
    }
    
    private void FetchLeaderboardData()
    {
        WWW www = new WWW(leaderBoardURL);
        StartCoroutine(GetData(www, (data) =>
        {
            string[] rows = data.Split("--");
            rows = rows[..^1]; // Take out the last element which is an empty string
            foreach (var row in rows)
            {
                string[] rowData = row.Split("**");
                leaderboardData.Add((rowData[0], int.Parse(rowData[1])));
            }
            PopulateLeaderboard();

        }));
        
    }
    
    private IEnumerator GetData(WWW www, Action<string> callback)
    {
        yield return www;
        callback(www.text);
    }
    
    void PopulateLeaderboard()
    {
        // Sort the leaderboard data by score
        leaderboardData.Sort((a, b) => b.Item2.CompareTo(a.Item2));
        int count = leaderboardData.Count > 5 ? 5 : leaderboardData.Count;
        leaderboardText.text = "";
        // Display the top 5 scores
        for (int i = 0; i < count; i++)
        {
            // Display the username and score
            Debug.Log($"{leaderboardData[i].Item1} - {leaderboardData[i].Item2}");
            leaderboardText.text += $"{leaderboardData[i].Item1} - {leaderboardData[i].Item2}\n";
        }
    }
    
    
    
}
