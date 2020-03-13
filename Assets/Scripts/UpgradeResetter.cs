using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeResetter : MonoBehaviour
{
    [SerializeField]
    private UpgradeObject[] upgrades;

    private MenuScores _scorekeeper;

    private void Start()
    {
        _scorekeeper = GameObject.FindObjectOfType<MenuScores>();
    }

    public void resetAll()
    {
        var refund = 0;
        foreach (var upgrade in upgrades)
        {
            refund += upgrade.sellAll();
        }
        _scorekeeper.refundCoins(refund);
    }
}
