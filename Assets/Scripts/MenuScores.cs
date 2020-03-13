using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuScores : MonoBehaviour
{
    public UnityEvent CoinsSpent;

    [SerializeField]
    private TextMeshProUGUI heighttext;
    [SerializeField]
    private TextMeshProUGUI cointext;
    [SerializeField]
    private TextMeshProUGUI timetext;

    [SerializeField]
    private ScoreSubmitter _scoreSubmitter = new ScoreSubmitter();

    private int _record = 0;
    private int _coins = 0;
    private int _maxcoins = 0;
    public int Coins { get { return _coins; } }
    
    void Start()
    {
        _record = PlayerPrefs.GetInt("Height", 0);
        heighttext.text = _record.ToString();

        _coins = PlayerPrefs.GetInt("Coins", 0);
        cointext.text = _coins.ToString();
        
        _maxcoins = PlayerPrefs.GetInt("MaxCoins", _coins);
        _scoreSubmitter.submitCoins(_maxcoins);
        _scoreSubmitter.submitHeight(_record);

        if(_record > 1200)
        {
            int prev_time = 0;
            bool success = SteamUserStats.GetStat("time", out prev_time);
            if(success && prev_time > 0)
            {
                timetext.text = TimeFormatter.Format(prev_time);
                timetext.enabled = true;
            }
        }
    }

    public void spendCoins(int coins)
    {
        if (_coins < coins)
            return;
        _coins -= coins;
        PlayerPrefs.SetInt("Coins", _coins);
        cointext.text = _coins.ToString();
        CoinsSpent.Invoke();
    }

    public void refundCoins(int coins)
    {
        _coins += coins;
        PlayerPrefs.SetInt("Coins", _coins);
        cointext.text = _coins.ToString();
        CoinsSpent.Invoke();
    }
}
