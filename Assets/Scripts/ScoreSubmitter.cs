using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class ScoreSubmitter
{
    private Leaderboard _coinboard = new Leaderboard("Max Coins Collected");
    private Leaderboard _heightboard = new Leaderboard("Max Height Reached");

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
}
