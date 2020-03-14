using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroLoader : MonoBehaviour
{
    enum type
    {
        intro,
        outro
    }

    [SerializeField]
    private type loadtype;

    [SerializeField]
    private VideoPlayer video;
    
    private float _alpha = 1f;

    [SerializeField]
    private float _fadespeed = 1f;

    private bool _end = false;
    
    void Start()
    {
        if(loadtype == type.intro)
        {
            video.url = Application.streamingAssetsPath + "/intro.m4v";
        }else
        {
            video.url = Application.streamingAssetsPath + "/outro.mp4";
        }

        video.Prepare();
        video.loopPointReached += setEnd;
    }

    void setEnd(VideoPlayer vp)
    {
        _end = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetButtonDown("Mute"))
            {
                video.SetDirectAudioMute(0, !video.GetDirectAudioMute(0));
            }
            else
            {
                _end = true;
            }
        }
        if (_end)
        {
            FadeOut();
            if (_alpha <= 0f)
                SceneManager.LoadScene("MainMenu");
        }
        else
            if (video.isPrepared && !video.isPlaying)
                video.Play();
    }

    void FadeOut()
    {
        _alpha -= _fadespeed * Time.deltaTime;
        _alpha = Mathf.Clamp01(_alpha);
        
        video.targetCameraAlpha = _alpha;
    }
}
