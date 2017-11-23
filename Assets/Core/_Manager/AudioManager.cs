using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    AudioSource _background;
    public AudioSource _ui;

    public bool IsMute
    {
        get
        {
            return _background.mute;
        }
    }


    public void SetMute(bool isMute)
    {
        _background.mute = isMute;
        _ui.mute = isMute;
    }

    public void Init(AudioSource background, AudioSource ui)
    {
        _background = background;
        _ui = ui;
    }

    public void SetBackgroundVolume(float value)
    {
        if (value > 0) SetMute(false);
        _background.volume = value;
    }

    public void SetUIVolume(float value)
    {
        if (value > 0) SetMute(false);
        _ui.volume = value;
    }

    public void PlayBackground(string clip, bool repeat = true)
    {
        _background.clip = ResourcesManager.Instance.LoadAssetByFullName<AudioClip>(clip);
        _background.loop = repeat;
        _background.Play();
    }

    public void PlayUIAudio(string clip)
    {
        _ui.clip = ResourcesManager.Instance.LoadAssetByFullName<AudioClip>(clip);
        _ui.loop = false;
        _ui.Play();
    }


}
