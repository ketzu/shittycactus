using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class KongAPIManager : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void InitKongAPI();

    [DllImport("__Internal")]
    private static extern void SubmitHeight(int height);

    [DllImport("__Internal")]
    private static extern void SubmitCoins(int coins);

    public void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        try{
            InitKongAPI();
        }catch {}
#endif
    }

    public void submitCoins(int coins)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        try{
        SubmitCoins(coins);
        }catch {}
#endif
    }

    public void submitHeight(int height)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        try{
        SubmitHeight(height);
        }catch {}
#endif
    }
}
