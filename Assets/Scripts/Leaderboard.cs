using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class Leaderboard
{
    private const ELeaderboardUploadScoreMethod _leaderboardMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest;

    private SteamLeaderboard_t _Leaderboard;

    private bool _initialized = false;
    private CallResult<LeaderboardFindResult_t> _findResult = new CallResult<LeaderboardFindResult_t>();
    private CallResult<LeaderboardScoreUploaded_t> _uploadResult = new CallResult<LeaderboardScoreUploaded_t>();

    private string name;

    private int unsubmitted = 0;

    public Leaderboard(string name)
    {
        this.name = name;
    }

    private void Init()
    {
        if (SteamManager.Initialized)
        {
            SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(name);
            _findResult.Set(hSteamAPICall, OnLeaderboardFindResult);
        }
    }

    public void UpdateScore(int score)
    {
        if (!_initialized)
        {
            UnityEngine.Debug.Log("Can't upload to the leaderboard because isn't loadded yet");
            Init();
            unsubmitted = score;
        }
        else
        {
            UnityEngine.Debug.Log("uploading score(" + score + ") to steam leaderboard(" + name + ")");
            if (SteamManager.Initialized)
            {
                SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(_Leaderboard, _leaderboardMethod, score, null, 0);
                _uploadResult.Set(hSteamAPICall, OnLeaderboardUploadResult);
            }
            else
            {
                unsubmitted = score;
            }
        }
    }

    private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool failure)
    {
        UnityEngine.Debug.Log("STEAM LEADERBOARDS: Found - " + pCallback.m_bLeaderboardFound + " leaderboardID - " + pCallback.m_hSteamLeaderboard.m_SteamLeaderboard);
        _Leaderboard = pCallback.m_hSteamLeaderboard;
        _initialized = true;

        if(unsubmitted != 0)
        {
            UpdateScore(unsubmitted);
            unsubmitted = 0;
        }
    }

    private void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t pCallback, bool failure)
    {
        UnityEngine.Debug.Log("STEAM LEADERBOARDS: failure - " + failure + " Completed - " + pCallback.m_bSuccess + " NewScore: " + pCallback.m_nGlobalRankNew + " Score " + pCallback.m_nScore + " HasChanged - " + pCallback.m_bScoreChanged);
    }
}