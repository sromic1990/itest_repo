using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class LeaderboardManagerAndroid : MonoBehaviour
{

    public string LeaderBoardID;

    #if UNITY_ANDROID
    public static LeaderboardManagerAndroid Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Create client configuration
        PlayGamesClientConfiguration config = new
            PlayGamesClientConfiguration.Builder()
            .Build();
        
        // Enable debugging output (recommended)
        PlayGamesPlatform.DebugLogEnabled = true;
        
        // Initialize and activate the platform
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        
        Authenticate();
    }

    private void Authenticate()
    {
        Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Successfully aunthenticated");
                }
                else
                {
                    Debug.Log("Not authenticated");
                }
            });
    }

    public void ShowLeaderBoard()
    {
        if (!Social.localUser.authenticated)
        {
            Authenticate();
        }
        PlayGamesPlatform.Instance.ShowLeaderboardUI(LeaderBoardID);
    }


    public string getUserName()
    {
        LeaderboardScoreData data = new LeaderboardScoreData(LeaderBoardID);
        string username = data.PlayerScore.userID;
        return username;
    }

    public void SubmitScore(int score)
    {
        Social.ReportScore(score, LeaderBoardID, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Posted");
                }
                else
                {
                    Debug.Log("Not Posted");
                }
            });
    }
        
    #endif
}
    
