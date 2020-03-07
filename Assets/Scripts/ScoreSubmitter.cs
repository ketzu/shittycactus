using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class ScoreSubmitter
{
    private Leaderboard _coinboard = new Leaderboard("Max Coins Collected");
    private Leaderboard _heightboard = new Leaderboard("Max Height Reached");
    private Leaderboard _timeboard = new Leaderboard("Lowest Time to 1200");

    public void submitCoins(int coins)
    {
        if (SteamManager.Initialized)
        {
            SteamUserStats.SetStat("coins", coins);
            _coinboard.UpdateScore(coins);
        }
    }

    public void submitHeight(int height)
    {
        if (SteamManager.Initialized)
        {
            SteamUserStats.SetStat("height", height);
            _heightboard.UpdateScore(height);
        }
    }
    public void submitTime(int time)
    {
        if (SteamManager.Initialized)
        {
            int prev_time;
            bool success = SteamUserStats.GetStat("time", out prev_time);
            if (success && prev_time > time)
            {
                SteamUserStats.SetStat("time", time);
                _coinboard.UpdateScore(time);
            }
        }
    }
}
