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
}
