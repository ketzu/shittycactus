using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Scores : MonoBehaviour
{
    public UnityEvent GoalReached;

    [SerializeField]
    private TextMeshProUGUI recordtext;
    [SerializeField]
    private TextMeshProUGUI heightendtext;
    [SerializeField]
    private TextMeshProUGUI heighttext;
    [SerializeField]
    private TextMeshProUGUI cointext;
    [SerializeField]
    private TextMeshProUGUI timetext;

    private ScoreSubmitter _scoreSub = new ScoreSubmitter();

    private int _record = 0;
    private int _maxheight = 0;
    private int _coins = 0;
    private int _maxcoins = 0;

    private int _maxrunheight = 0;

    private int _time = 0;

    private GameObject _player;

    private bool _race_finished = false;
    private float _goal_height;
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _player.GetComponent<PlayerController>().die.AddListener(onPlayerDeath);
        _goal_height = _player.GetComponent<PlayerController>().GoalHeight;

        _record = PlayerPrefs.GetInt("Height", 0);
        recordtext.text = _record.ToString();

        _coins = PlayerPrefs.GetInt("Coins", 0);
        _maxcoins = PlayerPrefs.GetInt("MaxCoins", _coins);
        cointext.text = _coins.ToString();

        _time = 0;
    }

    void onPlayerDeath()
    {
        _scoreSub.submitCoins(_maxcoins);
        _scoreSub.submitHeight(_record);

        _scoreSub.Sync();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_race_finished)
        {
            _time += (int)(Time.deltaTime * 1000);
            timetext.text = TimeFormatter.Format(_time);
        }

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
        if(height > _maxrunheight)
        {
            if(height > _goal_height && _maxrunheight <= _goal_height)
            {
                GoalReached.Invoke();
                _race_finished = true;
                _scoreSub.submitTime(_time);
            }
            _maxrunheight = height;
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
