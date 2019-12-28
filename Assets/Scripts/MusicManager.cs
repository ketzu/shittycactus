using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum type { effects, music };

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private type PlayerType;
    
    private AudioSource _audio_player;

    private Coroutine _active_play;

    // Start is called before the first frame update
    void Start()
    {
        _audio_player = GetComponent<AudioSource>();
        _audio_player.volume = PlayerPrefs.GetFloat(getSafeName(), 1f);
        _audio_player.mute = PlayerPrefs.GetInt(getSafeName()+"_mute", 0) == 1;
    }

    public void updateVolume()
    {
        if (_audio_player == null)
            _audio_player = GetComponent<AudioSource>();
        _audio_player.volume = PlayerPrefs.GetFloat(getSafeName(), 1f);
        _audio_player.mute = PlayerPrefs.GetInt(getSafeName() + "_mute", 0) == 1;
    }

    public void playDelayed(AudioClip clip)
    {
        if(_active_play != null)
        {
            StopCoroutine(_active_play);
        }
        _active_play = StartCoroutine(play(clip));
    }

    private IEnumerator play(AudioClip clip)
    {
        yield return new WaitForSeconds(0.2f);
        _audio_player.PlayOneShot(clip);
    }

    private string getSafeName()
    {
        if (PlayerType == type.effects)
            return "effects_volume";
        else
            return "music_volume";
    }
}
