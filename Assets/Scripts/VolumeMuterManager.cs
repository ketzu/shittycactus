using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VolumeMuterManager : MonoBehaviour
{
    public UnityEvent muteChanged;

    [SerializeField]
    private volume_names type;

    // Start is called before the first frame update
    void Start()
    {
        var toggle = GetComponent<Toggle>();
        toggle.isOn = PlayerPrefs.GetInt(getSafeName(), 0) == 1;
        toggle.onValueChanged.AddListener(setMute);
    }


    public void setMute(bool mute)
    {
        PlayerPrefs.SetInt(getSafeName(), mute ? 1 : 0);
        muteChanged.Invoke();
    }

    private string getSafeName()
    {
        if (type == volume_names.effects)
            return "effects_volume_mute";
        else
            return "music_volume_mute";
    }
}
