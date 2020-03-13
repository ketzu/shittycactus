using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField]
    private UpgradeObject up;
    [SerializeField]
    private CircularSliderHandler slider;
    [SerializeField]
    private TextMeshProUGUI costtext;
    [SerializeField]
    private SpriteRenderer coin;
    private MenuScores _scorekeeper;

    [SerializeField]
    private Button _default_on_deactivate;

    private void Start()
    {
        _scorekeeper = GameObject.FindObjectOfType<MenuScores>();
        if (_scorekeeper == null)
        {
            Debug.LogError("UpgradeButton: Could not find Scorekeeper.");
        }

        var button = GetComponent<Button>();
        button.onClick.AddListener(onClick);

        slider.minValue = up.min;
        slider.maxValue = up.max;

        updateText();
        updateSlider();
        checkValidity();
    }

    void onClick()
    {
        if (_scorekeeper.Coins >= up.getCost() && up.getCost() > 0)
        {
            _scorekeeper.spendCoins(up.getCost());
            up.buyCurrent();
            checkValidity();
            if (!GetComponent<Button>().interactable)
                _default_on_deactivate.Select();
        }
    }

    public void checkValidity()
    {
        updateText();
        updateSlider();
        var button = GetComponent<Button>();
        if (_scorekeeper.Coins < up.getCost() || up.getCost() <= 0)
            button.interactable = false;
        else
            button.interactable = true;
    }

    void updateText()
    {
        if (up.getCost() <= 0)
        {
            costtext.enabled = false;
            coin.enabled = false;
        }
        else
        {
            costtext.enabled = true;
            coin.enabled = true;
            costtext.text = up.getCost().ToString();
        }
    }

    void updateSlider()
    {
        slider.value = up.getValue();
    }


}
