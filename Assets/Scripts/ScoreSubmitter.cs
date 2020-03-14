using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class ScoreSubmitter
{
    private Leaderboard _coinboard = new Leaderboard("Max Coins Collected");
    private Leaderboard _heightboard = new Leaderboard("Max Height Reached");
    private Leaderboard _timeboard = new Leaderboard("Lowest Time to 1200");

    private bool _has_requested_stats = false;

    private Callback<UserStatsReceived_t> _UserStatsReceived;

    private bool _init = false;
    private int unsubmitted = 0;

    void Init()
    {
        if(!_init && SteamManager.Initialized)
        {
            _init = true;
            _UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnRestulReceived);
            SteamUserStats.RequestCurrentStats();
        }
    }

    public void Sync()
    {
        Init();
        if(SteamManager.Initialized)
        {
            SteamUserStats.StoreStats();
        }
    }

    void OnRestulReceived(UserStatsReceived_t param)
    {
        _has_requested_stats = true;
        if (unsubmitted != 0)
        {
            _SubmitTime(unsubmitted);
            unsubmitted = 0;
        }
        Sync();
    }

    public void submitCoins(int coins)
    {
        if (SteamManager.Initialized)
        {
            Init();
            SteamUserStats.SetStat("coins", coins);
            _coinboard.UpdateScore(coins);
        }
    }

    public void submitHeight(int height)
    {
        if (SteamManager.Initialized)
        {
            Init();
            SteamUserStats.SetStat("height", height);
            _heightboard.UpdateScore(height);
        }
    }
    public void submitTime(int time)
    {
        if (SteamManager.Initialized)
        {
            Init();
            if (_has_requested_stats)
            {
                _SubmitTime(time);
            }
            else
            {
                if(unsubmitted == 0)
                {
                    unsubmitted = time;
                }
                else if (time < unsubmitted)
                {
                    unsubmitted = time;
                }
            }
        }
    }

    private void _SubmitTime(int time)
    {
        if (SteamManager.Initialized)
        {
            int prev_time;
            bool success = SteamUserStats.GetStat("time", out prev_time);
            if (success && (prev_time > time || prev_time <= 0))
            {
                SteamUserStats.SetStat("time", time);
                _timeboard.UpdateScore(time);
                success = SteamUserStats.GetStat("time", out prev_time);
            }
        }
    }
}
