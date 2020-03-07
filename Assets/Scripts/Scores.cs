using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scores : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI recordtext;
    [SerializeField]
    private TextMeshProUGUI heightendtext;
    [SerializeField]
    private TextMeshProUGUI heighttext;
    [SerializeField]
    private TextMeshProUGUI cointext;

    private ScoreSubmitter _scoreSub = new ScoreSubmitter();

    private int _record = 0;
    private int _maxheight = 0;
    private int _coins = 0;
    private int _maxcoins = 0;

    private int _time = 0;

    private GameObject _player;
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _player.GetComponent<PlayerController>().die.AddListener(onPlayerDeath);

        _record = PlayerPrefs.GetInt("Height", 0);
        recordtext.text = _record.ToString();

        _coins = PlayerPrefs.GetInt("Coins", 0);
        _maxcoins = PlayerPrefs.GetInt("MaxCoins", _coins);
        cointext.text = _coins.ToString();


    }

    void onPlayerDeath()
    {
        _scoreSub.submitCoins(_maxcoins);
        _scoreSub.submitHeight(_record);
    }

    // Update is called once per frame
    void Update()
    {
        var height = Mathf.RoundToInt(_player.transform.position.y);
        if (height > _maxheight)
        {
            _maxheight = height;
            heighttext.text = _maxheight.ToString();
            heightendtext.text = _maxheight.ToString();
            if (height > _record)
            {
                _record = _maxheight;
                PlayerPrefs.SetInt("Height", height);
                recordtext.text = _record.ToString();
            }
        }
    }

    public void collectCoin()
    {
        _coins = PlayerPrefs.GetInt("Coins", 0) + 1;
        PlayerPrefs.SetInt("Coins", _coins);
        _maxcoins = PlayerPrefs.GetInt("MaxCoins", 0) + 1;
        PlayerPrefs.SetInt("MaxCoins", _maxcoins);
        cointext.text = _coins.ToString();
    }
}
