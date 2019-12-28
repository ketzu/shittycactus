using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum mode
{
    jumpheight,
    speed,
    initialplatforms,
    cloudrate,
    deathdepth,
    airdamping,
    coinprob
}

[CreateAssetMenu(fileName = "Upgradeable", menuName = "Upgradeable")]
public class UpgradeObject : ScriptableObject
{
    public float max = 0f;
    public float min = 0f;
    public int[] cost;
    public float[] values;

    [SerializeField]
    private mode Mode;

    public void buyCurrent()
    {
        var index = PlayerPrefs.GetInt(Mode.ToString(), -1);
        PlayerPrefs.SetInt(Mode.ToString(), index+1);
    }

    public float getValue()
    {
        var index = PlayerPrefs.GetInt(Mode.ToString(), -1);
        if (index < 0)
            return min;
        if (index > cost.Length)
            return max;
        return values[index];
    }

    public int getCost()
    {
        var index = PlayerPrefs.GetInt(Mode.ToString(), -1);
        if (index + 1 >= cost.Length)
            return -1;
        return cost[index+1];
    }
}
