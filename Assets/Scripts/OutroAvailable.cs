using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutroAvailable : MonoBehaviour
{
    void Start()
    {
        if(PlayerPrefs.GetInt("Final", 0) != 0)
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
