using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

enum volume_names
{
    effects,
    music
}

public class VolumeSliderManager : MonoBehaviour
{
    public UnityEvent volumeChanged;

    [SerializeField]
    private volume_names type;

    void Start()
    {
        var slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat(getSafeName(), 1f);
        slider.onValueChanged.AddListener(setVolume);
    }

    public void setVolume(float value)
    {
        if (value <= 0.01f)
            value = 0f;
        PlayerPrefs.SetFloat(getSafeName(), value);
        volumeChanged.Invoke();
    }

    private string getSafeName()
    {
        if (type == volume_names.effects)
            return "effects_volume";
        else
            return "music_volume";
    }
}
